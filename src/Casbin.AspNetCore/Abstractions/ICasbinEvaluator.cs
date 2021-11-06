using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinEvaluator
    {
        public Task<PolicyAuthorizationResult> AuthorizeAsync(ICasbinAuthorizationContext casbinContext,
            AuthorizationPolicy policy, AuthenticateResult? authenticationResult = null);
    }
}
