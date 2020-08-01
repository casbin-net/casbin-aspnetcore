using System;
using System.Security.Claims;
using Casbin.AspNetCore.Core.Abstractions;
using NetCasbin;
using NetCasbin.Model;

namespace Casbin.AspNetCore.Core
{
    public class CasbinAuthorizationCoreOptions
    {
        public string PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;
        public IRequestTransformer? DefaultRequestTransformer { get; set; }
        public Func<Model>? DefaultModelFactory { get; set; }
        public Func<Model?, Enforcer>? DefaultEnforcerFactory { get; set; }
    }
}
