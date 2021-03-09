using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, ICasbinAuthorizationData data,   HttpRequest? request = null);

        public ICasbinAuthorizationContext CreateContext(ClaimsPrincipal user, IEnumerable<ICasbinAuthorizationData> data, HttpRequest? request = null);
    }
}
