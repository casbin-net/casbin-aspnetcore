using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public class DefaultCasbinAuthorizationContextFactory : ICasbinAuthorizationContextFactory
    {
        public ICasbinAuthorizationContext CreateContext(
            ICasbinAuthorizationData data,  HttpContext httpContext)
            => new CasbinAuthorizationContext(data, httpContext);

        public ICasbinAuthorizationContext CreateContext(
            IEnumerable<ICasbinAuthorizationData> data,  HttpContext httpContext)
            => new CasbinAuthorizationContext(data, httpContext);
    }
}
