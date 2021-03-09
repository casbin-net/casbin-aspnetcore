using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContext
    {
        public ClaimsPrincipal User { get; }
        public IEnumerable<ICasbinAuthorizationData> AuthorizationData { get; }
        public HttpRequest? Request { get; }
    }
}
