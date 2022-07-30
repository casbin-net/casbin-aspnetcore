using System;
using System.Threading.Tasks;
using Casbin.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinEvaluator : ICasbinEvaluator
    {
        private readonly IAuthorizationService _authorizationService;

        public CasbinEvaluator(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync<TRequest>(ICasbinAuthorizationContext<TRequest> casbinContext, AuthorizationPolicy policy, AuthenticateResult? authenticationResult = null)
            where TRequest : IRequestValues
        {
            if (policy is null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            if (casbinContext is null)
            {
                throw new ArgumentNullException(nameof(casbinContext));
            }

            var result = await _authorizationService.AuthorizeAsync(casbinContext.HttpContext.User, casbinContext, policy);
            if (result.Succeeded)
            {
                return PolicyAuthorizationResult.Success();
            }

            if (authenticationResult is null)
            {
                return PolicyAuthorizationResult.Forbid();
            }

            // If authentication was successful, return forbidden, otherwise challenge
            return authenticationResult.Succeeded
                ? PolicyAuthorizationResult.Forbid()
                : PolicyAuthorizationResult.Challenge();
        }
    }
}
