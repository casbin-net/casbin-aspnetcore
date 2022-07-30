using System;
using Casbin.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Casbin.AspNetCore.Authorization
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="CasbinAuthorizationMiddleware"/> to the specified <see cref="IApplicationBuilder"/>, which enables authorization capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseCasbinAuthorization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CasbinAuthorizationMiddleware<RequestValues<string, string, string, string, string>>>();
        }

        /// <summary>
        /// Adds the <see cref="CasbinAuthorizationMiddleware"/> to the specified <see cref="IApplicationBuilder"/>, which enables authorization capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseCasbinAuthorization<TRequest>(this IApplicationBuilder app)
            where TRequest : IRequestValues
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            VerifyServicesRegistered(app);

            return app.UseMiddleware<CasbinAuthorizationMiddleware<TRequest>>();
        }

        private static void VerifyServicesRegistered(IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetService(typeof(ICasbinAuthorizationPolicyProvider)) == null)
            {
                throw new InvalidOperationException($"Unable to find the required services. Please add all the required services by calling '{nameof(IServiceCollection)}.{nameof(ServiceCollectionExtension.AddCasbinAuthorization)}' inside the call to 'ConfigureServices(...)' in the application startup code.");
            }
        }
    }
}
