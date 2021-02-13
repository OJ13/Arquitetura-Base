using DDD.Data;
using DDD.Domain.Models;

namespace DDD.Repositories
{
    public interface IPerfilRepository : IRepositoryBase<Perfil>
    {
    }
    public class PerfilRepository : RepositoryBase<Perfil>, IPerfilRepository
    {
        private readonly EFContext _eFContext;
        public PerfilRepository(EFContext eFContext) : base(eFContext)
        {
            _eFContext = eFContext;
        }
    }
}
