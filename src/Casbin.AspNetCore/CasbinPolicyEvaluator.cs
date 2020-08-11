using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Casbin.AspNetCore
{
    public class CasbinPolicyEvaluator : PolicyEvaluator
    {
        public CasbinPolicyEvaluator(IAuthorizationService authorization) : base(authorization)
        {
        }
    }
}
