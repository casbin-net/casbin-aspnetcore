using System.Collections.Generic;
using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization
{
    public class DefaultCasbinAuthorizationContextFactory : ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(
            ClaimsPrincipal user, ICasbinAuthorizationData data)
            => new CasbinAuthorizationContext(user, data);

        public ICasbinAuthorizationContext CreateContext(
            ClaimsPrincipal user, IEnumerable<ICasbinAuthorizationData> data)
            => new CasbinAuthorizationContext(user, data);
    }
}
