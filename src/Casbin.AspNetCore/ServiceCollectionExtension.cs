using Casbin.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Casbin.AspNetCore.Authorization
{
    public static class ServiceCollectionExtension
    {
        //private const string SuppressUseHttpContextAsAuthorizationResource = "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource";

        //public static IServiceCollection AddCasbinPolicy(this IServiceCollection services)
        //{
        //    AppContext.SetSwitch(SuppressUseHttpContextAsAuthorizationResource, true);
        //    return services;
        //}

        public static IServiceCollection AddCasbinAuthorization(this IServiceCollection services)
        {
            services.TryAddTransient<ICasbinEvaluator, CasbinEvaluator>();
            services.TryAddSingleton<ICasbinPolicyCreator, CasbinPolicyCreator>();
            services.TryAddSingleton<ICasbinAuthorizationMiddlewareResultHandler, CasbinAuthorizationMiddlewareResultHandler>();
            return services;
        }
    }
}
