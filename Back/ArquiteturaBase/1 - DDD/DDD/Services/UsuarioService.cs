using DDD.Domain.Models;
using DDD.Repositories;

namespace DDD.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario BuscaPorUserName(string userName);
    }
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository repositoryBase) : base(repositoryBase)
        {
            _usuarioRepository = repositoryBase;
        }

        public Usuario BuscaPorUserName(string userName)
        {
            return _usuarioRepository.BuscaPorUserName(userName);
        }
    }
}
