using System;
using System.Security.Claims;

namespace Casbin.AspNetCore.Abstractions
{
    public interface IRequestTransformer
    {
        public Func<ICasbinAuthorizationContext, string> SubTransformer { get; set; }
        public Func<ICasbinAuthorizationContext, object> ObjTransformer { get; set; }
        public Func<ICasbinAuthorizationContext, string> ActTransformer { get; set; }
        public string? Issuer { get; }
    }
}
