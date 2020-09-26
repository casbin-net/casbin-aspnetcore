using System;
using System.IO;
using Microsoft.Extensions.Options;
using NetCasbin;
using NetCasbin.Model;
using NetCasbin.Persist.FileAdapter;

namespace Casbin.AspNetCore.Authorization
{
    public class DefaultEnforcerProvider : IEnforcerProvider
    {
        private readonly IOptions<CasbinAuthorizationOptions> _options;
        private readonly ICasbinModelProvider _modelProvider;
        private Enforcer? _enforcer;

        public DefaultEnforcerProvider(IOptions<CasbinAuthorizationOptions> options,
            ICasbinModelProvider modelProvider)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _modelProvider = modelProvider ?? throw new ArgumentNullException(nameof(modelProvider));
        }

        public virtual Enforcer? GetEnforcer()
        {
            if (_enforcer is not null)
            {
                return _enforcer;
            }

            if (_options.Value.DefaultEnforcerFactory is not null)
            {
                _enforcer ??= _options.Value.DefaultEnforcerFactory(_modelProvider.GetModel());
                return _enforcer;
            }

            Model? model = _modelProvider.GetModel();
            if (model is null)
            {
                throw new ArgumentException($"{_modelProvider.GetModel()} can not return null when {nameof(_options.Value.DefaultEnforcerFactory)} option is empty");
            }

            string? policyPath = _options.Value.DefaultPolicyPath;
            if (policyPath is not null)
            {
                if (!File.Exists(policyPath))
                {
                    throw new FileNotFoundException("Can not find the policy file path.", policyPath);
                }
                _enforcer ??= new Enforcer(_modelProvider.GetModel(), new DefaultFileAdapter(policyPath));
                return _enforcer;
            }

            _enforcer ??= new Enforcer(_modelProvider.GetModel());
            return _enforcer;
        }
    }
}
