using System;
using Casbin.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Casbin.AspNetCore.Authorization
{
    public static class ServiceCollectionExtension
    {
        private const string SuppressUseHttpContextAsAuthorizationResource = "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource";

        // This method try to support casbin by no new middleware.
        internal static IServiceCollection AddCasbinPolicy(this IServiceCollection services)
        {
            AppContext.SetSwitch(SuppressUseHttpContextAsAuthorizationResource, true);
            return services;
        }

        public static IServiceCollection AddCasbinAuthorization(this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configure = default,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped)
        {
            services.TryAddTransient<ICasbinEvaluator, CasbinEvaluator>();
            services.TryAddSingleton<ICasbinPolicyCreator, CasbinPolicyCreator>();
            services.TryAddSingleton<ICasbinAuthorizationMiddlewareResultHandler, CasbinAuthorizationMiddlewareResultHandler>();
            services.AddCasbinAuthorizationCore(configure, defaultModelProviderLifeTime, defaultEnforcerProviderLifeTime);
            return services;
        }
    }
}
