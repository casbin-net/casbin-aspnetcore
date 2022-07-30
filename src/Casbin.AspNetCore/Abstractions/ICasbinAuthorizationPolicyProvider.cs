using System.Collections.Generic;
using Casbin.Model;
using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationPolicyProvider
    {
        public AuthorizationPolicy GetAuthorizationPolicy<TRequest>(IEnumerable<ICasbinAuthorizationData<TRequest>> authorizationData)
            where TRequest : IRequestValues;
    }
}
