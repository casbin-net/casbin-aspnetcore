using System;
using Casbin.AspNetCore.Abstractions;
using Microsoft.Extensions.Options;
using NetCasbin;

namespace Casbin.AspNetCore
{
    public class DefaultEnforcerProvider : IEnforcerProvider
    {
        private readonly IOptions<CasbinAuthorizationCoreOptions> _options;
        private readonly ICasbinModelProvider _casbinModelProvider;
        private Enforcer? _enforcer;

        public DefaultEnforcerProvider(IOptions<CasbinAuthorizationCoreOptions> options,
            ICasbinModelProvider casbinModelProvider)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _casbinModelProvider = casbinModelProvider ?? throw new ArgumentNullException(nameof(casbinModelProvider));
        }

        public virtual Enforcer? GetEnforcer()
        {
            _enforcer ??= _options.Value.DefaultEnforcerFactory?.Invoke(_casbinModelProvider.GetModel());;
            return _enforcer;
        }
    }
}
