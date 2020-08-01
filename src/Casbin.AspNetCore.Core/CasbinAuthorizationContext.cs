using System.Security.Claims;
using Casbin.AspNetCore.Core.Abstractions;

namespace Casbin.AspNetCore.Core
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
