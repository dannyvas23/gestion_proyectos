using Application.CasosDeUso;
using Domain.Puertos;
using Application.Interfaces;
using Infrastructure.Persistencia.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.ComunicacionContinua;

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
