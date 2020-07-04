using Microsoft.Extensions.DependencyInjection;

namespace Casbin.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCasbinAuthorization(this IServiceCollection services)
        {

            return services;
        }
    }
}
