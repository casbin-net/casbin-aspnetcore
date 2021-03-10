using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Casbin.AspNetCore.Authorization
{
    public class CasbinAuthorizationMiddleware
    {
        // AppContext switch used to control whether HttpContext or endpoint is passed as a resource to AuthZ
        private const string SuppressUseHttpContextAsAuthorizationResource = "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource";

        // Property key is used by Endpoint routing to determine if Authorization has run
        private const string AuthorizationMiddlewareInvokedWithEndpointKey = "__AuthorizationMiddlewareWithEndpointInvoked";
        private static readonly object _authorizationMiddlewareWithEndpointInvokedValue = new object();

        private readonly RequestDelegate _next;
        private readonly ICasbinPolicyCreator _policyCreator;

        public CasbinAuthorizationMiddleware(RequestDelegate next, ICasbinPolicyCreator policyCreator)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _policyCreator = policyCreator ?? throw new ArgumentNullException(nameof(policyCreator)); ;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                // EndpointRoutingMiddleware uses this flag to check if the Authorization middleware processed auth metadata on the endpoint.
                // The Authorization middleware can only make this claim if it observes an actual endpoint.
                context.Items[AuthorizationMiddlewareInvokedWithEndpointKey] = _authorizationMiddlewareWithEndpointInvokedValue;
            }

            // IMPORTANT: Changes to authorization logic should be mirrored in MVC's AuthorizeFilter
            var authorizeData = endpoint?.Metadata.GetOrderedMetadata<ICasbinAuthorizationData>();

            if (authorizeData is null || authorizeData.Count == 0)
            {
                await _next(context);
                return;
            }


            // Policy evaluator has transient lifetime so it fetched from request services instead of injecting in constructor
            var policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();

            var policy = _policyCreator.Create(authorizeData);
            var authenticateResult = await policyEvaluator.AuthenticateAsync(policy, context);

            // Allow Anonymous skips all authorization
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            object? resource;
            if (AppContext.TryGetSwitch(SuppressUseHttpContextAsAuthorizationResource, out bool useEndpointAsResource) && useEndpointAsResource)
            {
                resource = endpoint;
            }
            else
            {
                resource = context;
            }

            var casbinContext = context.RequestServices.GetRequiredService<ICasbinAuthorizationContextFactory>().CreateContext(authorizeData, context);

            var authorizeResult = await context.RequestServices.GetRequiredService<ICasbinEvaluator>().AuthorizeAsync(policy, authenticateResult, context, casbinContext, resource);

            var authorizationMiddlewareResultHandler = context.RequestServices.GetRequiredService<ICasbinAuthorizationMiddlewareResultHandler>();
            await authorizationMiddlewareResultHandler.HandleAsync(_next, context, policy, authorizeResult);
        }
    }
}
