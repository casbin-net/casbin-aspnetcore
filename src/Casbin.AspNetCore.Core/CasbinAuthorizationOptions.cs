using System;
using System.Security.Claims;
using Casbin;
using Casbin.AspNetCore.Authorization.Transformers;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationOptions
    {
        public string? DefaultModelPath { get; set; }
        public string? DefaultPolicyPath { get; set; }
        public Func<IServiceProvider, IModel?, Enforcer>? DefaultEnforcerFactory { get; set; }
        public string? DefaultAuthenticationSchemes { get; set; }
        public Type? DefaultRequestTransformerType { get; set; } = typeof(BasicRequestTransformer);
        public string PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;
        public bool AllowAnyone { get; set; } = false;
    }
}
