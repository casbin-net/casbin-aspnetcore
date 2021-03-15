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

        /// <summary>
        /// Adds casbin authorization services to the specified <see cref="IServiceCollection" />
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configureOptions"></param>
        /// <param name="defaultEnforcerProviderLifeTime">The lifetime with which to register the IEnforcerProvider service in the container.</param>
        /// <param name="defaultModelProviderLifeTime">The lifetime with which to register the ICasbinModelProvider service in the container.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCasbinAuthorization(this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configureOptions = default,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped)
        {
            services.TryAddTransient<ICasbinEvaluator, CasbinEvaluator>();
            services.TryAddSingleton<ICasbinAuthorizationPolicyProvider, DefaultCasbinAuthorizationPolicyProvider>();
            services.TryAddSingleton<ICasbinAuthorizationMiddlewareResultHandler, CasbinAuthorizationMiddlewareResultHandler>();
            services.AddCasbinAuthorizationCore(configureOptions, defaultEnforcerProviderLifeTime, defaultModelProviderLifeTime);
            return services;
        }
    }
}
