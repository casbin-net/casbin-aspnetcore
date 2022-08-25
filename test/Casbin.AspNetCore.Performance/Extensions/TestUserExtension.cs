using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Casbin.AspNetCore.Performance.Extensions
{
    public static class UserExtension
    {
        public static HttpContext CreateDefaultHttpContext(this ClaimsPrincipal user)
        {
            return new DefaultHttpContext
            {
                User = user
            };
        }

        public static HttpContext CreateDefaultHttpContext(this ClaimsPrincipal user, string path, HttpMethod httpMethod)
        {
            HttpContext context = CreateDefaultHttpContext(user);
            context.Request.Path = path;
            context.Request.Method = httpMethod.ToString();
            return context;
        }
    }
}
