using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinPolicyCreator
    {
        public AuthorizationPolicy Create(IEnumerable<ICasbinAuthorizationData> authorizationData);
    }
}
