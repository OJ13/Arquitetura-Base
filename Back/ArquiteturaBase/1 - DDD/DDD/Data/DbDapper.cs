using DDD.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace DDD.Data
{
    public class DbDapper
    {
        public DbDapper(IOptions<ApplicationSettings> applicationSettings)
        {
            _databaseSettings = applicationSettings.Value.DatabaseSettings;
        }

        protected DatabaseSettings _databaseSettings;
        protected DbConnection GetDefaultConnection()
        {
            return new SqlConnection(_databaseSettings.ConnectionStrings);
        }
    }
    public class ServiceDapper<TEntity> : IServiceDapper<TEntity> where TEntity : class
    {
        private readonly IRepositoryDapper<TEntity> _repositoryDapper;
        public ServiceDapper(IRepositoryDapper<TEntity> repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

    }
    public interface IServiceDapper<TEntity> where TEntity : class { }
    public interface IRepositoryDapper<TEntity> where TEntity : class { }
}
