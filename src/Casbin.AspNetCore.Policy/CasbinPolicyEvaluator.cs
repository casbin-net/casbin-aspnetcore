using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Casbin.AspNetCore.Policy
{
    public class CasbinPolicyEvaluator : PolicyEvaluator
    {
        public CasbinPolicyEvaluator(IAuthorizationService authorization) : base(authorization)
        {
        }
    }
}
