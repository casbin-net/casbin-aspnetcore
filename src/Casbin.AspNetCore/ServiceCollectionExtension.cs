using System;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Policy;
using Casbin.AspNetCore.Transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCasbin;
using NetCasbin.Model;

namespace Casbin.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCasbinAuthorizationCore(this IServiceCollection services, Action<CasbinAuthorizationCoreOptions> configure)
        {
            var options = CasbinAuthorizationCoreOptions.GetDefault();
            configure(options);
            CheckOptions(options);
            services.TryAdd(ServiceDescriptor.Describe(typeof(Model),
                p => options.ModelFactory?.Invoke(),
                options.ModelLifeTime));
            services.TryAdd(ServiceDescriptor.Describe(typeof(Enforcer),
                p =>
                    options.EnforcerFactory?.Invoke(p.GetRequiredService<Model>()),
                options.EnforcerLifeTime));
            services.TryAddSingleton<
                ICasbinAuthorizationContextFactory,
                DefaultCasbinAuthorizationContextFactory>();
            services.TryAddScoped<IAuthorizationHandler, CasbinAuthorizationHandler>();
            services.AddSingleton<IRequestTransformer, BasicRequestTransformer>();
            services.AddAuthorizationCore();
            return services;
        }

        private static void CheckOptions(CasbinAuthorizationCoreOptions options)
        {
            if (options.ModelFactory is null)
            {
                throw new ArgumentNullException(nameof(options.ModelFactory));
            }

            if (options.EnforcerFactory is null)
            {
                throw new ArgumentNullException(nameof(options.EnforcerFactory));
            }
        }
    }
}
