using System;
using Microsoft.Extensions.DependencyInjection;
using NetCasbin;
using NetCasbin.Model;

namespace Casbin.AspNetCore
{
    public class CasbinAuthorizationCoreOptions
    {
        public ServiceLifetime ModelLifeTime { get; set; }
        public ServiceLifetime EnforcerLifeTime { get; set; }
        public Func<Model>? ModelFactory { get; set; }
        public Func<Model, Enforcer>? EnforcerFactory { get; set; }

        public static CasbinAuthorizationCoreOptions GetDefault()
        {
            return new CasbinAuthorizationCoreOptions
            {
                ModelLifeTime = ServiceLifetime.Scoped,
                EnforcerLifeTime = ServiceLifetime.Scoped,
            };
        }
    }
}
