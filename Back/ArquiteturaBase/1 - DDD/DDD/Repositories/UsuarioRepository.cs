using DDD.Data;
using DDD.Domain.Models;
using System.Linq;

namespace DDD.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Usuario BuscaPorUserName(string userName);
    }
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        private readonly EFContext _context;

        public UsuarioRepository(EFContext context) : base(context)
        {
            _context = context;
        }

        public Usuario BuscaPorUserName(string userName)
        {
            return _context.Usuario.Where(p => p.UserName == userName).FirstOrDefault();
        }
    }
}
