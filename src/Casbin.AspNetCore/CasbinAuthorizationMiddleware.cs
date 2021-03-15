using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationMiddleware
    {
        private const string s_casbinAuthorizationMiddlewareInvokedWithEndpointKey = "__CasbinAuthorizationMiddlewareWithEndpointInvoked";
        private static readonly object s_casbinAuthorizationMiddlewareWithEndpointInvokedValue = new();

        private readonly RequestDelegate _next;
        private readonly ICasbinAuthorizationPolicyProvider _policyCreator;
        private readonly IOptions<CasbinAuthorizationOptions> _options;

        public CasbinAuthorizationMiddleware(RequestDelegate next, ICasbinAuthorizationPolicyProvider policyCreator, IOptions<CasbinAuthorizationOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _policyCreator = policyCreator ?? throw new ArgumentNullException(nameof(policyCreator));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpoint = context.GetEndpoint();
            if (endpoint is not null)
            {
                context.Items[s_casbinAuthorizationMiddlewareInvokedWithEndpointKey] = s_casbinAuthorizationMiddlewareWithEndpointInvokedValue;
            }

            // IMPORTANT: Changes to authorization logic should be mirrored in MVC's AuthorizeFilter
            var authorizeData = endpoint?.Metadata.GetOrderedMetadata<ICasbinAuthorizationData>();

            if (authorizeData is null || authorizeData.Count == 0)
            {
                await _next(context);
                return;
            }

            bool allowAnyone = _options.Value.AllowAnyone;
            var policy = _policyCreator.GetAuthorizationPolicy(authorizeData);

            AuthenticateResult? authenticateResult = null;
            if (allowAnyone is false)
            {
                // Policy evaluator has transient lifetime so it fetched from request services instead of injecting in constructor
                var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();
                authenticateResult = await policyEvaluator.AuthenticateAsync(policy, context);
            }

            // Allow Anonymous skips all authorization
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
            {
                await _next(context);
                return;
            }

            var casbinAuthorizationContextFactory = context.RequestServices.GetRequiredService<ICasbinAuthorizationContextFactory>();
            var casbinAuthorizationContext = casbinAuthorizationContextFactory.CreateContext(authorizeData, context);

            var casbinEvaluator = context.RequestServices.GetRequiredService<ICasbinEvaluator>();
            var authorizeResult = await casbinEvaluator.AuthorizeAsync(casbinAuthorizationContext, policy, authenticateResult);

            var authorizationMiddlewareResultHandler = context.RequestServices.GetRequiredService<ICasbinAuthorizationMiddlewareResultHandler>();
            await authorizationMiddlewareResultHandler.HandleAsync(_next, context, policy, authorizeResult);
        }
    }
}
