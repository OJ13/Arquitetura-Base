using API.Models.DTO;
using API.Models.Response;
using DDD.Domain.Models;
using DDD.Helpers;
using DDD.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ApplicationSettings _appSettings;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<Perfil> _roleManager;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IPasswordValidator<Usuario> _passwordValidator;
        private readonly IUsuarioSenhaHistoryService _usuarioSenhaHistoryService;
        public AccountController(
              ILogger<AccountController> logger
            , IOptions<ApplicationSettings> appsettings
            , UserManager<Usuario> userManager
            , SignInManager<Usuario> signInManager
            , RoleManager<Perfil> roleManager
            , IPasswordHasher<Usuario> passwordHasher
            , IPasswordValidator<Usuario> passwordValidator
            , IUsuarioSenhaHistoryService usuarioSenhaHistoryService
            ) :base(logger)
        {
            _logger = logger;
            _appSettings = appsettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _usuarioSenhaHistoryService = usuarioSenhaHistoryService;
        }
        [HttpGet]
        [Route("config")]
        public async Task<IActionResult> Config()
        {
            return Ok("Identity Config 1.0");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDTO request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                    return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Autenticação do Usuário") { Errors = new List<string>() { "Usuário não reconhecido pela aplicação" } });

                if (user.Ativo)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, request.Senha, isPersistent: false, lockoutOnFailure: true);

                    if (!result.IsLockedOut)
                    {
                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user);

                            user.Logado = true;
                            user.DataUltimoLogin = DateTime.Now;
                            await _userManager.UpdateAsync(user);

                            var userAutentication = new UserAutenticationDTO();
                            if (user.ForcaTrocaSenha)
                            {
                                userAutentication.UserName = user.UserName;
                                userAutentication.DataLogin = user.DataUltimoLogin;
                                userAutentication.Autenticated = false;
                                userAutentication.ForcaTrocaSenha = user.ForcaTrocaSenha;
                            }
                            else
                            {
                                var buildToken = TokenHelper.BuildToken(user, roles.ToList(), _appSettings.Auth.JWTSecret, _appSettings.Auth.Expiration);
                                userAutentication.UserName = user.UserName;
                                userAutentication.DataLogin = user.DataUltimoLogin;
                                userAutentication.Token = buildToken;
                                userAutentication.Autenticated = true;
                                userAutentication.ForcaTrocaSenha = user.ForcaTrocaSenha;
                            }

                            return Ok(new ApiResponseSuccess<UserAutenticationDTO>(userAutentication, "Usuário Autenticado com sucesso!"));
                        }
                        else
                        {
                            return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Autenticação do Usuário") { Errors = new List<string>() { "Usuário ou Senha Inválidos" } });
                        }
                    }
                    else
                    {
                        return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Autenticação do Usuário") { Errors = new List<string>() { "Usuário bloqueado por excesso de tentativas (Aguarde 30 Minutos para Tentar Novamente)" } });
                    }
                }
                else
                {
                    return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Autenticação do Usuário") { Errors = new List<string>() { "Usuário Desativado, entre em contato com o Administrador para ativação do usuário" } });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao Autenticar {message}", ex.Message);
                return NotFound(new ApiResponseError(HttpStatusCode.NotFound, "Erro na Autenticação do Usuário") { Errors = new List<string>() { $"Erro ao Autenticar" } });
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] UserLogoutDTO request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    var user = await _userManager.FindByNameAsync(request.UserName);
                    if (user != null)
                    {
                        user.Logado = false;

                        var deslogar = await _userManager.UpdateAsync(user);
                        await _signInManager.SignOutAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                _logger.LogError("Erro inesperado: {erro}", erro);
            }

            _logger.LogInformation("Usuário Deslogado");
            return Ok(new ApiResponseSuccess<bool>(true));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Usuario request)
        {
            try
            {
                if (request.PerfilId == null)
                    return BadRequest("Nenhum Perfil Associado!");

                request.ForcaTrocaSenha = true;
                request.DataCriacao = DateTime.Now;
                request.Logado = false;
                request.Ativo = true;
                request.DataUltimaTrocaSenha = DateTime.Now;
                request.DataUltimoLogin = DateTime.Now;

                var existUser = await _userManager.FindByNameAsync(request.UserName);

                if (existUser == null)
                {
                    var result = await _userManager.CreateAsync(request, request.PasswordHash);

                    if (result.Succeeded)
                    {
                        var perfil = await _roleManager.FindByIdAsync(request.PerfilId.ToString());

                        if (perfil != null)
                        {
                            await _userManager.AddToRoleAsync(request, perfil.Name);
                        }

                        var user = await _userManager.FindByIdAsync(request.Id.ToString());

                        var usuarioSenhaHistory = new UsuarioSenhaHistory()
                        {
                            UsuarioId = user.Id,
                            DataCriacao = DateTime.Now,
                            PasswordHash = user.PasswordHash
                        };

                        _usuarioSenhaHistoryService.Add(usuarioSenhaHistory);

                        return Ok(new ApiResponseSuccess<Usuario>(user, "Usuário cadastrado com sucesso!"));
                    }
                    else
                    {
                        return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Criação de Usuários") { Errors = result.Errors.Select(p => p.Description).ToList() });
                    }
                }
                else
                {
                    return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Usuário já Existente na base") { Errors = new List<string>() { "Usuário já existente na base" } });
                }

            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponseError(HttpStatusCode.BadRequest, "Erro genérico na Criação de Usuários") { Errors = new List<string>() { $"Erro: {ex.Message}" } });
            }
        }

        [HttpPost]
        [Route("createPerfil")]
        public async Task<IActionResult> CreatePerfil([FromBody] Perfil request)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(request.Name);
                if (!roleExist)
                {
                    var perfil = new Perfil() { Name = request.Name, Ativo = true };
                    await _roleManager.CreateAsync(perfil);

                    return Ok(perfil);
                }
                else
                {
                    return BadRequest("Perfil já existente!");
                }
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Criação do Perfil") { Errors = new List<string>() { $"Erro: {ex.Message}" } });
            }
        }

        [HttpPost]
        [Route("modifyPassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UserRedefinicaoSenhaDTO request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user != null)
                {
                    var validarNovaSenha = _passwordValidator.ValidateAsync(_userManager, user, request.NovaSenha);

                    if (!validarNovaSenha.Result.Succeeded)
                        return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Modificação da Senha") { Errors = validarNovaSenha.Result.Errors.Select(p => p.Description).ToList() });

                    var autenticado = await _signInManager.PasswordSignInAsync(user, request.SenhaAtual, isPersistent: false, lockoutOnFailure: false);
                    if (!autenticado.Succeeded)
                        return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Usuário ou Senha Inválidos") { Errors = new List<string>() { "Usuário ou Senha Inválidos" } });

                    var verificarUltimasSenhas = _usuarioSenhaHistoryService.UltimasSenhas(user.Id, _appSettings.Auth.QtdSenhasVerificar);
                    foreach (var item in verificarUltimasSenhas)
                    {
                        var igualUltimasTres = _passwordHasher.VerifyHashedPassword(null, item.PasswordHash, request.NovaSenha);
                        if (igualUltimasTres == PasswordVerificationResult.Success)
                        {
                            return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Senha não pode ser igual as últimas 3 Senhas válidas") { Errors = new List<string>() { "Senha não pode ser igual as últimas 3 Senhas válidas" } });
                        }
                    }

                    var hashPass = _passwordHasher.HashPassword(user, request.NovaSenha);
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, hashPass);

                    if (result.Succeeded)
                    {
                        user.DataUltimaTrocaSenha = DateTime.Now;
                        user.FirstLogin = false;
                        user.ForcaTrocaSenha = false;
                        user.Logado = true;
                        user.DataUltimoLogin = DateTime.Now;
                        user.PasswordHash = hashPass;

                        await _userManager.UpdateAsync(user);

                        var usuarioSenhaHistory = new UsuarioSenhaHistory()
                        {
                            UsuarioId = user.Id,
                            DataCriacao = DateTime.Now,
                            PasswordHash = user.PasswordHash
                        };

                        _usuarioSenhaHistoryService.Add(usuarioSenhaHistory);

                        var roles = await _userManager.GetRolesAsync(user);
                        var buildToken = TokenHelper.BuildToken(user, roles.ToList(), _appSettings.Auth.JWTSecret, _appSettings.Auth.Expiration);

                        var userAutentication = new UserAutenticationDTO()
                        {
                            UserName = user.UserName,
                            DataLogin = user.DataUltimoLogin,
                            Token = buildToken,
                            Autenticated = true,
                            ForcaTrocaSenha = false
                        };

                        return Ok(new ApiResponseSuccess<UserAutenticationDTO>(userAutentication, "Senha Redefina com Sucesso!"));
                    }
                    else
                    {
                        return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Modificação da Senha") { Errors = result.Errors.Select(p => p.Description).ToList() });
                    }
                }
                else
                {
                    return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Usuário ou Senha Inválidos") { Errors = new List<string>() { "Usuário ou Senha Inválidos" } });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao Modificar a Senha {message}", ex.Message);
                return NotFound(new ApiResponseError(HttpStatusCode.NotFound, "Erro na Troca de Senha") { Errors = new List<string>() { $"Erro ao tentar Trocar a Senha" } });
            }
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> EsqueciMinhaSenha([FromBody] UserEsqueciMinhaSenhaDTO request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user != null)
                {
                    var hashPass = _passwordHasher.HashPassword(user, request.SenhaProvisoria);
                    await _userManager.RemovePasswordAsync(user);
                    var result = await _userManager.AddPasswordAsync(user, hashPass);

                    if (result.Succeeded)
                    {
                        user.DataUltimaTrocaSenha = DateTime.Now;
                        user.FirstLogin = false;
                        user.ForcaTrocaSenha = true;
                        user.Logado = false;
                        user.PasswordHash = hashPass;

                        await _userManager.UpdateAsync(user);

                        return Ok(new ApiResponseSuccess<Usuario>(user, "Senha Redefina com Sucesso!"));
                    }
                    else
                    {
                        return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Erro na Modificação da Senha") { Errors = result.Errors.Select(p => p.Description).ToList() });
                    }
                }
                else
                {
                    return BadRequest(new ApiResponseError(HttpStatusCode.BadRequest, "Usuário ou Senha Inválidos") { Errors = new List<string>() { "Usuário ou Senha Inválidos" } });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no Esqueci Minha Senha {message}", ex.Message);
                return NotFound(new ApiResponseError(HttpStatusCode.NotFound, "Erro no Esqueci Minha Senha") { Errors = new List<string>() { $"Erro no Esqueci Minha Senha" } });
            }
        }

    }
}