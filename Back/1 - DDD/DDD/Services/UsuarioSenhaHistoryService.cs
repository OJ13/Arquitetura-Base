using DDD.Domain.Models;
using DDD.Repositories;
using System;
using System.Collections.Generic;

namespace DDD.Services
{
    public interface IUsuarioSenhaHistoryService : IServiceBase<UsuarioSenhaHistory>
    {
        List<UsuarioSenhaHistory> UltimasSenhas(Guid usuarioId, int qtdHaVerificar);
    }
    public class UsuarioSenhaHistoryService : ServiceBase<UsuarioSenhaHistory>, IUsuarioSenhaHistoryService
    {
        private readonly IUsuarioSenhaHistoryRepository _usuarioSenhaHistoryRepository;
        public UsuarioSenhaHistoryService(IUsuarioSenhaHistoryRepository repositoryBase) : base(repositoryBase)
        {
            _usuarioSenhaHistoryRepository = repositoryBase;
        }

        public List<UsuarioSenhaHistory> UltimasSenhas(Guid usuarioId, int qtdHaVerificar)
        {
            return _usuarioSenhaHistoryRepository.UltimasSenhas(usuarioId, qtdHaVerificar);
        }
    }
}
