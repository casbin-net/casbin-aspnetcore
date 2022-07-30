using System.Collections.Generic;
using System.Security.Claims;
using Casbin.Model;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContextFactory<TRequest> where TRequest : IRequestValues
    {
        public ICasbinAuthorizationContext<TRequest> CreateContext(ICasbinAuthorizationData<TRequest> data, HttpContext httpContext);

        public ICasbinAuthorizationContext<TRequest> CreateContext(IEnumerable<ICasbinAuthorizationData<TRequest>> data, HttpContext httpContext);
    }
}
