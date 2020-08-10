using System;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Authorization.Policy;
using Casbin.AspNetCore.Authorization.Transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Casbin.AspNetCore.Authorization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCasbinAuthorizationCore(
            this IServiceCollection services,
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
            services.TryAddSingleton<
                ICasbinAuthorizationContextFactory,
                DefaultCasbinAuthorizationContextFactory>();
            services.TryAddScoped<IEnforceService, DefaultEnforcerService>();
            services.TryAddSingleton<IRequestTransformersCache, RequestTransformersCache>();
            services.AddScoped<IAuthorizationHandler, CasbinAuthorizationHandler>();
            services.AddSingleton<IRequestTransformer, BasicRequestTransformer>();
            services.AddAuthorizationCore();
            return services;
        }
    }
}
