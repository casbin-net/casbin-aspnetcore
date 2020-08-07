using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinPolicyCreator
    {
        public AuthorizationPolicy Create(ICasbinAuthorizationData authorizationData);
    }
}
