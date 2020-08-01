using System;
using Casbin.AspNetCore.Core.Abstractions;
using Microsoft.Extensions.Options;
using NetCasbin.Model;

namespace Casbin.AspNetCore.Core
{
    public class DefaultCasbinModelProvider : ICasbinModelProvider
    {
        private readonly IOptions<CasbinAuthorizationCoreOptions> _options;
        private Model? _model;

        public DefaultCasbinModelProvider(IOptions<CasbinAuthorizationCoreOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public virtual Model? GetModel()
        {
            _model ??= _options.Value.DefaultModelFactory?.Invoke();
            return _model;
        }
    }
}
