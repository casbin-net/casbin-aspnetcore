using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationContext : ICasbinAuthorizationContext
    {
        public CasbinAuthorizationContext(ClaimsPrincipal user, ICasbinAuthorizationData data, HttpRequest? request = null)
            : this(user, new[]{ data }, request)
        {
        }

        public CasbinAuthorizationContext(ClaimsPrincipal user, IEnumerable<ICasbinAuthorizationData> data, HttpRequest? request = null)
        {
            User = user;
            AuthorizationData = data;
            Request = request;
        }

        public ClaimsPrincipal User { get; }
        public IEnumerable<ICasbinAuthorizationData> AuthorizationData { get; }

        public HttpRequest? Request { get; }

    }
}
