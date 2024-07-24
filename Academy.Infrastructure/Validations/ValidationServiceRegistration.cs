using MediatR;
using System.Reflection;

namespace Academy.Infrastructure.Validations
{
    public static class ValidationServiceRegistration
    {
        public static IServiceCollection AddBehaviours(this IServiceCollection services, Assembly assemblyContainingValidators)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}