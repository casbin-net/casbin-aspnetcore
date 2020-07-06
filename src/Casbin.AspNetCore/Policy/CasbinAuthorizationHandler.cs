using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Casbin.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Authorization;
using NetCasbin;

namespace Casbin.AspNetCore.Policy
{
    public class CasbinAuthorizationHandler : AuthorizationHandler<CasbinAuthorizationRequirement, ICasbinAuthorizationContext>
    {
        private readonly IEnforceService _enforcerService;

        public CasbinAuthorizationHandler(IEnforceService enforcerService)
        {
            _enforcerService = enforcerService ?? throw new ArgumentNullException(nameof(enforcerService));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CasbinAuthorizationRequirement requirement,
            ICasbinAuthorizationContext casbinContext)
        {
            if (await _enforcerService.EnforceAsync(casbinContext))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
