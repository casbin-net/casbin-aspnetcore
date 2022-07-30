using System.Collections.Generic;
using Casbin.Model;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationContext<TRequest> where TRequest : IRequestValues
    {
        public IEnumerable<ICasbinAuthorizationData<TRequest>> AuthorizationData { get; }
        public HttpContext HttpContext { get; }
    }
}
