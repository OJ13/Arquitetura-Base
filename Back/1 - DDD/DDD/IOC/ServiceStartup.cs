using DDD.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.IOC
{
    public static class ServiceStartup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<IUsuarioSenhaHistoryService, UsuarioSenhaHistoryService>();
        }

    }
}
