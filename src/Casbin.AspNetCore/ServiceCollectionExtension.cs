using System;
using Microsoft.Extensions.DependencyInjection;

namespace Casbin.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        private const string SuppressUseHttpContextAsAuthorizationResource = "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource";

        public static IServiceCollection AddCasbinPolicy(this IServiceCollection services)
        {
            AppContext.SetSwitch(SuppressUseHttpContextAsAuthorizationResource, true);
            return services;
        }
    }
}
