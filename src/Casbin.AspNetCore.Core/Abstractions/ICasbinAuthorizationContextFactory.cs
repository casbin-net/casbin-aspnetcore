using System.Security.Claims;

namespace Casbin.AspNetCore.Core.Abstractions
{
    public interface ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, ICasbinAuthorizationData data);
    }
}
