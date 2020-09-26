using System;
using System.Linq;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Casbin.AspNetCore.Authorization
{
    public class DefaultEnforcerService : IEnforceService
    {
        private readonly IOptions<CasbinAuthorizationOptions> _options;
        private readonly IRequestTransformersCache _transformersCache;
        private readonly IEnforcerProvider _enforcerProvider;
        private readonly ILogger<DefaultEnforcerService> _logger;

        public DefaultEnforcerService(
            IOptions<CasbinAuthorizationOptions> options,
            IRequestTransformersCache transformersCache,
            IEnforcerProvider enforcerProvider,
            ILogger<DefaultEnforcerService> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _transformersCache = transformersCache ?? throw new ArgumentNullException(nameof(transformersCache));
            _enforcerProvider = enforcerProvider ?? throw new ArgumentNullException(nameof(enforcerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<bool> EnforceAsync(ICasbinAuthorizationContext context)
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

            foreach (var data in context.AuthorizationData)
            {
                // The order of decide transformer is :
                // 1. context.Data.RequestTransformerType >
                // 2. _options.Value.DefaultRequestTransformer >
                // 3. _transformers.FirstOrDefault()
                IRequestTransformer? transformer = null;
                if (data.RequestTransformerType is not null)
                {
                    transformer = transformersArray.FirstOrDefault( t =>
                        t.GetType() == data.RequestTransformerType);
                    if (transformer is null)
                    {
                        throw new ArgumentException("Can find any specified type request transformer.", nameof(data.RequestTransformerType));
                    }
                }
                else if (!noDefault)
                {
                    transformer = _options.Value.DefaultRequestTransformer;
                }

                transformer ??= transformersArray.FirstOrDefault();

                if (transformer is null)
                {
                    throw new ArgumentException("Can find any request transformer.", nameof(_transformersCache.Transformers));
                }

                // The order of deciding transformer.PreferSubClaimType is :
                // 1. context.Data.PreferSubClaimType >
                // 2. _options.Value.PreferSubClaimType
                transformer.PreferSubClaimType = data.PreferSubClaimType ?? _options.Value.PreferSubClaimType;

                // The order of deciding transformer.PreferSubClaimType is :
                // 1. context.Data.PreferSubClaimType >
                // 2. null (if this issuer is null, it will be ignored)
                transformer.Issuer = data.Issuer;

                var requestValues = await transformer.TransformAsync(context, data);

                if (enforcer.Enforce(requestValues as object[] ?? requestValues.ToArray()))
                {
                    _logger.CasbinAuthorizationSucceeded();
                    continue;
                }

                _logger.CasbinAuthorizationFailed();
                return false;
            }

            return true;
        }
    }
}
