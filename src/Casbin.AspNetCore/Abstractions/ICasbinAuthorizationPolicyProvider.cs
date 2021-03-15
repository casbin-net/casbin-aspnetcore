using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationPolicyProvider
    {
        public AuthorizationPolicy GetAuthorizationPolicy(IEnumerable<ICasbinAuthorizationData> authorizationData);
    }
}
