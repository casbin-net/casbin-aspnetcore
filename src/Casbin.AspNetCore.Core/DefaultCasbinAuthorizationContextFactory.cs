using System.Security.Claims;
using Casbin.AspNetCore.Core.Abstractions;

namespace Casbin.AspNetCore.Core
{
    public class DefaultCasbinAuthorizationContextFactory : ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(
            ClaimsPrincipal user, ICasbinAuthorizationData data)
            => new CasbinAuthorizationContext(user, data);
    }
}
