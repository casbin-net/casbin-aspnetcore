using System.Collections.Generic;
using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, ICasbinAuthorizationData data);

        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, IEnumerable<ICasbinAuthorizationData> data);
    }
}
