using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinEvaluator
    {
        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy authorizationPolicy,
            AuthenticateResult authenticationResult, HttpContext context,
            ICasbinAuthorizationContext casbinContext, object? resource);
    }
}
