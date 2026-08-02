using Application.CasosDeUso;
using Domain.Puertos;
using GestionProyectos.Application.CasosDeUso;
using GestionProyectos.Application.Interfaces;
using GestionProyectos.Domain.Puertos;
using GestionProyectos.Infrastructure.Persistencia.Repositorios;
using GestionProyectos.Infrastructure.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace gestion_proyectos_api
{
    public static class ConfiguracionServicio
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //Comunicacion continua
            services.AddScoped<IServicioTablero, ServicioTablero>();

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

            // --- SignalR ---
            services.AddSignalR();
            return services;
        }
    }
}
