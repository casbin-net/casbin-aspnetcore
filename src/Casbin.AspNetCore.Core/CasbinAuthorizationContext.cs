using System.Collections.Generic;
using System.Security.Claims;
using Casbin.Model;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationContext<TRequest> : ICasbinAuthorizationContext<TRequest> where TRequest : IRequestValues
    {
        public CasbinAuthorizationContext(ICasbinAuthorizationData<TRequest> data, HttpContext httpContext)
            : this(new[]{ data }, httpContext)
        {
        }

        public CasbinAuthorizationContext(IEnumerable<ICasbinAuthorizationData<TRequest>> data, HttpContext httpContext)
        {
            AuthorizationData = data;
            HttpContext = httpContext;
        }


        public IEnumerable<ICasbinAuthorizationData<TRequest>> AuthorizationData { get; }

        public HttpContext HttpContext { get; }

    }
}
