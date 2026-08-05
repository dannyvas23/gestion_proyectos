using Application.Comun;
using Microsoft.OpenApi.Models;

namespace gestion_proyectos_api
{
    public static class ConfiguracionServicio
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration ApiSettingConfig)
        {
            services.Configure<ApiSettings>(ApiSettingConfig.GetSection("ApiSettings"));
            services.Configure<ApiSettings>(ApiSettingConfig.GetSection("JwtConfig"));

            // Swagger con soporte JWT
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Gestión de Proyectos Ágiles",
                    Version = "v1",
                    Description = "API REST para gestión de proyectos con tablero Kanban"
                });

                // Configurar autenticación JWT en Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization. Escriba: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            return services;
        }
    }
}
