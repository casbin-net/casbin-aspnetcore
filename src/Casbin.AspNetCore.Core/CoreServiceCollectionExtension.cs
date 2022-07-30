using System;
using Casbin.AspNetCore.Authorization.Policy;
using Casbin.AspNetCore.Authorization.Transformers;
using Casbin.Model;
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
        /// <param name="configureOptions"></param>
        /// <param name="defaultEnforcerProviderLifeTime">The lifetime with which to register the IEnforcerProvider service in the container.</param>
        /// <param name="defaultModelProviderLifeTime">The lifetime with which to register the ICasbinModelProvider service in the container.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCasbin(this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configureOptions = default,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped)
        {
            services.Configure(configureOptions);
            services.TryAdd(ServiceDescriptor.Describe(
                typeof(ICasbinModelProvider), typeof(DefaultCasbinModelProvider),
                defaultModelProviderLifeTime));
            services.TryAdd(ServiceDescriptor.Describe(
                typeof(IEnforcerProvider), typeof(DefaultEnforcerProvider),
                defaultEnforcerProviderLifeTime));
            return services;
        }

                /// <summary>
        /// Adds casbin authorization core services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configureOptions"></param>
        /// <param name="defaultEnforcerProviderLifeTime">The lifetime with which to register the IEnforcerProvider service in the container.</param>
        /// <param name="defaultModelProviderLifeTime">The lifetime with which to register the ICasbinModelProvider service in the container.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCasbinAuthorizationCore(
            this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configureOptions = default,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped)
        {
            services.AddCasbinAuthorizationCore<StringRequestValues>(configureOptions, defaultEnforcerProviderLifeTime, defaultModelProviderLifeTime);
            return services;
        }

        /// <summary>
        /// Adds casbin authorization core services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configureOptions"></param>
        /// <param name="defaultEnforcerProviderLifeTime">The lifetime with which to register the IEnforcerProvider service in the container.</param>
        /// <param name="defaultModelProviderLifeTime">The lifetime with which to register the ICasbinModelProvider service in the container.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCasbinAuthorizationCore<TRequest>(
            this IServiceCollection services,
            Action<CasbinAuthorizationOptions>? configureOptions = default,
            ServiceLifetime defaultEnforcerProviderLifeTime = ServiceLifetime.Scoped,
            ServiceLifetime defaultModelProviderLifeTime = ServiceLifetime.Scoped)
            where TRequest : IRequestValues
        {
            services.AddCasbin(configureOptions, defaultEnforcerProviderLifeTime, defaultModelProviderLifeTime);
            services.TryAdd(ServiceDescriptor.Singleton(
                typeof(ICasbinAuthorizationContextFactory<>),
                typeof(DefaultCasbinAuthorizationContextFactory<>)));
            services.TryAddScoped<IEnforceService, DefaultEnforcerService>();
            services.TryAdd(ServiceDescriptor.Singleton(
                typeof(IRequestTransformersCache<>),
                typeof(RequestTransformersCache<>)));
            // Can not change to TryAdd, because there interface may need more than one implement.
            services.AddScoped<IAuthorizationHandler, CasbinAuthorizationHandler<TRequest>>();
            services.AddSingleton<IRequestTransformer<StringRequestValues>, BasicRequestTransformer>();
            services.AddSingleton<IRequestTransformer<StringRequestValues>, RbacRequestTransformer>();
            services.AddSingleton<IRequestTransformer<StringRequestValues>, KeyMatchRequestTransformer>();

            services.AddAuthorizationCore();
            return services;
        }
    }
}
