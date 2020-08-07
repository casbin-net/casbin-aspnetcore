using System.Collections.Generic;
using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationContext : ICasbinAuthorizationContext
    {
        public CasbinAuthorizationContext(ClaimsPrincipal user, ICasbinAuthorizationData data)
            : this(user, new[]{ data })
        {
        }

        public CasbinAuthorizationContext(ClaimsPrincipal user, IEnumerable<ICasbinAuthorizationData> data)
        {
            User = user;
            AuthorizationData = data;
        }

        public ClaimsPrincipal User { get; }
        public IEnumerable<ICasbinAuthorizationData> AuthorizationData { get; }
    }
}
