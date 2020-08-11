using System;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Authorization.Policy;
using Casbin.AspNetCore.Authorization.Transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Casbin.AspNetCore.Authorization
{
    public static class CoreServiceCollectionExtension
    {
        /// <summary>
        /// Adds casbin core services to the specified <see cref="IServiceCollection" />
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure"></param>
        /// <param name="defaultModelProviderLifeTime"></param>
        /// <param name="defaultEnforcerProviderLifeTime"></param>
        /// <returns></returns>
        public static IServiceCollection AddCasbin(this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configure = default,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped)
        {
            services.Configure(configure);
            services.TryAdd(ServiceDescriptor.Describe(
                typeof(ICasbinModelProvider), typeof(DefaultCasbinModelProvider),
                defaultModelProviderLifeTime));
            services.TryAdd(ServiceDescriptor.Describe(
                typeof(IEnforcerProvider), typeof(DefaultEnforcerProvider),
                defaultEnforcerProviderLifeTime));
            return services;
        }

        /// <summary>
        /// Adds casbin authorization services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure"></param>
        /// <param name="defaultModelProviderLifeTime"></param>
        /// <param name="defaultEnforcerProviderLifeTime"></param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCasbinAuthorizationCore(
            this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configure = default,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped)
        {
            services.AddCasbin(configure, defaultModelProviderLifeTime, defaultEnforcerProviderLifeTime);
            services.TryAddSingleton<ICasbinAuthorizationContextFactory, DefaultCasbinAuthorizationContextFactory>();
            services.TryAddScoped<IEnforceService, DefaultEnforcerService>();
            services.TryAddSingleton<IRequestTransformersCache, RequestTransformersCache>();
            services.AddScoped<IAuthorizationHandler, CasbinAuthorizationHandler>();
            services.AddSingleton<IRequestTransformer, BasicRequestTransformer>();
            services.AddAuthorizationCore();
            return services;
        }
    }
}
