using System;
using System.Linq;
using System.Threading.Tasks;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Casbin.AspNetCore.Core
{
    public class DefaultEnforcerService : IEnforceService
    {
        private readonly IOptions<CasbinAuthorizationCoreOptions> _options;
        private readonly IRequestTransformersCache _transformersCache;
        private readonly IEnforcerProvider _enforcerProvider;
        private readonly ILogger<DefaultEnforcerService> _logger;

        public DefaultEnforcerService(
            IOptions<CasbinAuthorizationCoreOptions> options,
            IRequestTransformersCache transformersCache,
            IEnforcerProvider enforcerProvider,
            ILogger<DefaultEnforcerService> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(CasbinAuthorizationCoreOptions));
            _transformersCache = transformersCache ?? throw new ArgumentNullException(nameof(transformersCache));
            _enforcerProvider = enforcerProvider ?? throw new ArgumentNullException(nameof(enforcerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual Task<bool> EnforceAsync(ICasbinAuthorizationContext context)
        {
            var enforcer = _enforcerProvider.GetEnforcer();
            if (enforcer is null)
            {
                throw new ArgumentException("Can not find any enforcer.");
            }

            var transformersArray =
                _transformersCache.Transformers as IRequestTransformer[] ??
                _transformersCache.Transformers?.ToArray();

            bool noDefault = _options.Value.DefaultRequestTransformer is null;
            if (transformersArray is null || transformersArray.Length == 0 && noDefault)
            {
                throw new ArgumentException("Can find any request transformer.");
            }

            // The order of decide transformer is :
            // 1. context.Data.RequestTransformerType >
            // 2. _options.Value.DefaultRequestTransformer >
            // 3. _transformers.FirstOrDefault()
            IRequestTransformer? transformer = null;
            if (!(context.Data.RequestTransformerType is null))
            {
                transformer = transformersArray.FirstOrDefault( t => t.GetType() == context.Data.RequestTransformerType);
            }
            else if (!noDefault)
            {
                transformer = _options.Value.DefaultRequestTransformer;
            }
            transformer ??= transformersArray.FirstOrDefault();

            // The order of deciding transformer.PreferSubClaimType is :
            // 1. context.Data.PreferSubClaimType >
            // 2. _options.Value.PreferSubClaimType
            transformer.PreferSubClaimType = context.Data.PreferSubClaimType ?? _options.Value.PreferSubClaimType;

            // The order of deciding transformer.PreferSubClaimType is :
            // 1. context.Data.PreferSubClaimType >
            // 2. null (if this issuer is null, it will be ignored)
            transformer.Issuer = context.Data.Issuer;

            var requestValues = transformer.Transform(context);

            if (enforcer.Enforce(requestValues as object[] ?? requestValues.ToArray()))
            {
                _logger.CasbinAuthorizationSucceeded();
                return Task.FromResult(true);
            }

            _logger.CasbinAuthorizationFailed();
            return Task.FromResult(false);
        }
    }
}
