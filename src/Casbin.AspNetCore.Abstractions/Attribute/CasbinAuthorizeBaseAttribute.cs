using System;

namespace Casbin.AspNetCore.Authorization
{
    public abstract class CasbinAuthorizeBaseAttribute : Attribute
    {
        public virtual string? Issuer { get; set; }
        public virtual string? PreferSubClaimType { get; set; }
        public virtual Type? RequestTransformerType { get; set; }
        public virtual string? AuthenticationSchemes { get; set; }
    }
}
