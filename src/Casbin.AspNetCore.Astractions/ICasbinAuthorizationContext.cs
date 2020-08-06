using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContext
    {
        public ClaimsPrincipal User { get; }
        public ICasbinAuthorizationData Data { get; }
    }
}
