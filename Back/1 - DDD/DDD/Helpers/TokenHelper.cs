using DDD.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DDD.Helpers
{
    public static class TokenHelper
    {
        public static string BuildToken(Usuario user, List<string> roles, string keyApp, double timeExpiration)
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keyApp);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim("PerfilId", user.PerfilId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(timeExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescription);

            return tokenHandle.WriteToken(token);
        }
    }
}
