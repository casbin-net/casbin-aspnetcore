using System;
using System.Security.Claims;

namespace Casbin.AspNetCore.Abstractions
{
    public interface ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, ICasbinAuthorizationData data);
        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, ICasbinAuthorizationData data, Type requestTransformerType);
    }
}
