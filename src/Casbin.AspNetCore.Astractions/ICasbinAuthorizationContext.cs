using System.Collections.Generic;
using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContext
    {
        public ClaimsPrincipal User { get; }
        public IEnumerable<ICasbinAuthorizationData> AuthorizationData { get; }
    }
}
