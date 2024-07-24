using Academy.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Academy.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddSingleton(typeof(IAIStringLocalizer<>), typeof(AIStringLocalizer<>));
            var assembly = Assembly.GetExecutingAssembly();
            MapsterSettings.Configure();

            services.AddValidatorsFromAssembly(assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });

            return services;
        }
    }
}
