using Application.CasosDeUso;
using GestionProyectos.Domain.Puertos;
using GestionProyectos.Infrastructure.Persistencia.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace gestion_proyectos_api
{
    public static class ConfiguracionServicio
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Repositorios
            services.AddScoped<IProyectoRepositorio, ProyectoRepositorio>();

            //Casos de uso
            services.AddScoped<ProyectoUC>();
            return services;
        }
    }
}
