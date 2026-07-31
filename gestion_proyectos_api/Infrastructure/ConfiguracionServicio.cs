using GestionProyectos.Domain.Puertos;
using GestionProyectos.Infrastructure.Persistencia.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace gestion_proyectos_api
{
    public static class ConfiguracionServicio
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IProyectoRepositorio, ProyectoRepositorio>();
            return services;
        }
    }
}
