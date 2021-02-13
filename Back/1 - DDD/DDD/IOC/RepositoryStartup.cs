using DDD.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.IOC
{
    public static class RepositoryStartup
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IPerfilRepository, PerfilRepository>();
        }
    }
}
