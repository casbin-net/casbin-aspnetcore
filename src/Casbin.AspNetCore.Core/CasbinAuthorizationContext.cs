using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationContext : ICasbinAuthorizationContext
    {
        public CasbinAuthorizationContext(ClaimsPrincipal user, ICasbinAuthorizationData data)
        {
            User = user;
            Data = data;
        }

        public ClaimsPrincipal User { get; }
        public ICasbinAuthorizationData Data { get; }
    }
}
