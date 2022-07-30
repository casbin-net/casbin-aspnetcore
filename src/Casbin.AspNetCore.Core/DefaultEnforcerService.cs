using System;
using System.Linq;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization.Extensions;
using Casbin.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Casbin.AspNetCore.Authorization
{
    public class DefaultEnforcerService : IEnforceService
    {
        private readonly IOptions<CasbinAuthorizationOptions> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnforcerProvider _enforcerProvider;
        private readonly ILogger<DefaultEnforcerService> _logger;

        public DefaultEnforcerService(
            IOptions<CasbinAuthorizationOptions> options,
            IServiceProvider serviceProvider,
            IEnforcerProvider enforcerProvider,
            ILogger<DefaultEnforcerService> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _enforcerProvider = enforcerProvider ?? throw new ArgumentNullException(nameof(enforcerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<bool> EnforceAsync<TRequest>(ICasbinAuthorizationContext<TRequest> context)
            where TRequest : IRequestValues
        {
            var enforcer = _enforcerProvider.GetEnforcer();
            if (enforcer is null)
            {
                throw new ArgumentException("Cannot find any enforcer.");
            }

            var transformersCache = _serviceProvider.GetService<IRequestTransformersCache<TRequest>>();
            if (transformersCache is null)
            {
                throw new ArgumentException("Cannot find the specific request values type transformers cache.");
            }
            var transformers = transformersCache.Transformers;
            bool noDefault = _options.Value.DefaultRequestTransformerType is null;
            if (transformers is null || transformers.Count is 0 && noDefault)
            {
                throw new ArgumentException($"Cannot find the specific request values type request transformer.");
            }

            foreach (var data in context.AuthorizationData)
            {
                // The order of decide transformer is :
                // 1. context.Data.RequestTransformerType >
                // 2. _options.Value.DefaultRequestTransformerType >
                // 3. _transformers.FirstOrDefault()
                IRequestTransformer<TRequest>? transformer = null;
                if (data.RequestTransformerType is not null)
                {
                    if (transformersCache.TryGetTransformer(data.RequestTransformerType, out transformer) is false)
                    {
                        throw new ArgumentException($"Cannot find request transformer {data.RequestTransformerType}.");
                    }
                }
                else if (_options.Value.DefaultRequestTransformerType is not null)
                {
                    if (transformersCache.TryGetTransformer(_options.Value.DefaultRequestTransformerType, out transformer) is false)
                    {
                        throw new ArgumentException($"Cannot find request transformer {_options.Value.DefaultRequestTransformerType}.");
                    }
                }
                else
                {
                    if (transformers.Count is not 1)
                    {
                        throw new ArgumentException("Cannot determine the exclusive request transformer.");
                    }
                    transformer = transformers[0];
                }

                if (transformer is null)
                {
                    throw new ArgumentException($"Cannot find the specific request values type request transformer.");
                }

                // The order of deciding transformer.PreferSubClaimType is :
                // 1. context.Data.PreferSubClaimType >
                // 2. _options.Value.PreferSubClaimType
                transformer.PreferSubClaimType = data.PreferSubClaimType ?? _options.Value.PreferSubClaimType;

                // The order of deciding transformer.Issuer is :
                // 1. context.Data.Issuer >
                // 2. null (if this issuer is null, it will be ignored)
                transformer.Issuer = data.Issuer;

                if(data.Values is null)
                {
                    throw new ArgumentException("Cannot find any request value.");
                }
                TRequest values = await transformer.TransformAsync(context, data);
                if(await enforcer.EnforceAsync(enforcer.CreateContext(), values))
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
