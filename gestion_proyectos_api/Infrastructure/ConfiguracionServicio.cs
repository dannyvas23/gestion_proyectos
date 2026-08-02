using Application.CasosDeUso;
using Domain.Puertos;
using GestionProyectos.Application.CasosDeUso;
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
            services.AddScoped<IColumnaRepositorio, ColumnaRepositorio>();
            services.AddScoped<ITareaRepositorio, TareaRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

            //Casos de uso
            services.AddScoped<ProyectoUC>();
            services.AddScoped<ColumnaUC>();
            services.AddScoped<TareaUC>();
            services.AddScoped<UsuarioUC>();
            return services;
        }
    }
}
