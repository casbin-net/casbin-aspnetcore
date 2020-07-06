using System;
using System.Security.Claims;

namespace Casbin.AspNetCore.Abstractions
{
    public interface IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public string SubTransform(ICasbinAuthorizationContext context);
        public object ObjTransform(ICasbinAuthorizationContext context);
        public string ActTransform(ICasbinAuthorizationContext context);
    }
}
