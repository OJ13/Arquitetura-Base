using DDD.Data;
using DDD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Repositories
{
    public interface IUsuarioSenhaHistoryRepository : IRepositoryBase<UsuarioSenhaHistory>
    {
        List<UsuarioSenhaHistory> UltimasSenhas(Guid usuarioId, int qtdHaVerificar);
    }
    public class UsuarioSenhaHistoryRepository : RepositoryBase<UsuarioSenhaHistory>, IUsuarioSenhaHistoryRepository
    {
        private readonly EFContext _context;

        public UsuarioSenhaHistoryRepository(EFContext context) : base(context)
        {
            _context = context;
        }

        public List<UsuarioSenhaHistory> UltimasSenhas(Guid usuarioId, int qtdHaVerificar)
        {
            var query = _context.UsuarioSenhaHistory.Where(p => p.UsuarioId == usuarioId).OrderByDescending(p => p.DataCriacao).Take(qtdHaVerificar);

            return query.ToList();
        }
    }
}
