using System.Linq;

namespace DDD.Helpers
{
    public static class PaginacaoHelper<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> Paginar(IQueryable<TEntity> query, int regInicial, int qntRegistros)
        {
            query = query.Skip(regInicial);

            if (qntRegistros > 0)
                query = query.Take(qntRegistros);

            return query;
        }
    }
}
