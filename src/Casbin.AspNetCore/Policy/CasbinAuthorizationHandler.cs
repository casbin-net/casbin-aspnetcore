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
        private readonly Enforcer _enforcer;
        private readonly IEnumerable<IRequestTransformer> _transformers;

        public CasbinAuthorizationHandler(Enforcer enforcer, IEnumerable<IRequestTransformer> transformers)
        {
            _enforcer = enforcer ?? throw new ArgumentNullException(nameof(enforcer));
            _transformers = transformers ?? throw new ArgumentNullException(nameof(transformers));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CasbinAuthorizationRequirement requirement,
            ICasbinAuthorizationContext casbinContext)
        {
            bool noDefault = requirement.DefaultRequestTransformer is null;
            var transformersArray = _transformers.ToArray();
            if (transformersArray.Length == 0 && noDefault)
            {
                throw new ArgumentException("Can find any request transformer.");
            }

            IRequestTransformer? transformer = null;
            if (!(casbinContext.RequestTransformerType is null))
            {
                transformer = _transformers.FirstOrDefault( t => t.GetType() == casbinContext.RequestTransformerType);
            }
            else if (!noDefault)
            {
                transformer = requirement.DefaultRequestTransformer;
            }

            transformer ??= _transformers.FirstOrDefault();

            string? sub = transformer.SubTransformer(casbinContext);
            object? obj = transformer.ObjTransformer(casbinContext);
            string? act = transformer.ActTransformer(casbinContext);

            if (_enforcer.Enforce(sub, obj, act))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
