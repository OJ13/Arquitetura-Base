using DDD.Domain.Models;
using DDD.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Services
{
    public interface IPerfilService : IServiceBase<Perfil>
    {
    }
    public class PerfilService : ServiceBase<Perfil>, IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;

        public PerfilService(IPerfilRepository perfilRepository) : base(perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }
    }
}
