using System;
using Microsoft.AspNetCore.Builder;

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
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            VerifyServicesRegistered(app);

            return app.UseMiddleware<CasbinAuthorizationMiddleware>();
        }

        private static void VerifyServicesRegistered(IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetService(typeof(ICasbinPolicyCreator)) == null)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
