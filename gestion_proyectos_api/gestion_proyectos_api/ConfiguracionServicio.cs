using Application.Comun;

namespace gestion_proyectos_api
{
    public static class ConfiguracionServicio
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration ApiSettingConfig)
        {
            services.Configure<ApiSettings>(ApiSettingConfig.GetSection("ApiSettings"));
            return services;
        }
    }
}
