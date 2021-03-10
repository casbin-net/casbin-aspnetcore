using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationContext : ICasbinAuthorizationContext
    {
        public CasbinAuthorizationContext(ICasbinAuthorizationData data, HttpContext httpContext)
            : this(new[]{ data }, httpContext)
        {
        }

        public CasbinAuthorizationContext(IEnumerable<ICasbinAuthorizationData> data, HttpContext httpContext)
        {
            AuthorizationData = data;
            HttpContext = httpContext;
        }


        public IEnumerable<ICasbinAuthorizationData> AuthorizationData { get; }

        public HttpContext HttpContext { get; }

    }
}
