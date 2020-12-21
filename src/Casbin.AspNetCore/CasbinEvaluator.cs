using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinEvaluator : ICasbinEvaluator
    {
        private readonly IAuthorizationService _authorizationService;

        public CasbinEvaluator(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context,
            ICasbinAuthorizationContext casbinContext, object? resource)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            if (casbinContext == null)
            {
                throw new ArgumentNullException(nameof(casbinContext));
            }

            var result = await _authorizationService.AuthorizeAsync(context.User, casbinContext, policy);
            if (result.Succeeded)
            {
                return PolicyAuthorizationResult.Success();
            }

            // If authentication was successful, return forbidden, otherwise challenge
            return authenticationResult.Succeeded
                ? PolicyAuthorizationResult.Forbid()
                : PolicyAuthorizationResult.Challenge();
        }
    }
}
