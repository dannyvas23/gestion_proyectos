using Application.CasosDeUso;
using Application.Interfaces;
using Domain.Puertos;
using Infrastructure.Auth;
using Infrastructure.ComunicacionContinua;
using Infrastructure.Persistencia;
using Infrastructure.Persistencia.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace gestion_proyectos_api
{
    public static class ConfiguracionServicio
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Base de datos
            services.AddDbContext<AppDbContext>(options =>
                 options.UseNpgsql(config.GetConnectionString("PostgresDatabase"))
            );

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
            services.AddScoped<AuthUC>();

            //Servicios
            services.AddScoped<IServicioAuth, ServicioAuth>();


            // Autenticación JWT ---
            var jwtKey = config["JwtConfig:Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtConfig:Issuer"],
                    ValidAudience = config["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                // Permitir token en query string para SignalR
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        // Si la petición es para el Hub de SignalR, tomar token del query string
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub/tablero"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });


            // --- SignalR ---
            services.AddSignalR();
            return services;
        }
    }
}
