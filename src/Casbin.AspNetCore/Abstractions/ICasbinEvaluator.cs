using System.Threading.Tasks;
using Casbin.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinEvaluator
    {
        public Task<PolicyAuthorizationResult> AuthorizeAsync<TRequest>(ICasbinAuthorizationContext<TRequest> casbinContext,
            AuthorizationPolicy policy, AuthenticateResult? authenticationResult = null)
            where TRequest : IRequestValues;
    }
}
