using System.Security.Claims;

namespace Casbin.AspNetCore.Abstractions
{
    public interface ICasbinAuthorizationContext
    {
        public ClaimsPrincipal User { get; }
        public ICasbinAuthorizationData Data { get; }
    }
}
