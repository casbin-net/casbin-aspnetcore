using System;
using System.IO;
using Microsoft.Extensions.Options;
using NetCasbin;
using NetCasbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public class DefaultCasbinModelProvider : ICasbinModelProvider
    {
        private readonly string _fallbackModelPath = "model.conf";
        private readonly IOptions<CasbinAuthorizationOptions> _options;
        private Model? _model;

        public DefaultCasbinModelProvider(IOptions<CasbinAuthorizationOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public virtual Model? GetModel()
        {
            if (_model is not null)
            {
                return _model;
            }

            string? modelPath = _options.Value.DefaultModelPath;

            if (string.IsNullOrWhiteSpace(modelPath))
            {
                if (_options.Value.DefaultEnforcerFactory is not null)
                {
                    return null;
                }
                modelPath = _fallbackModelPath;
            }

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException("Can not find the model file path.", modelPath);
            }

            _model ??= Model.CreateDefaultFromFile(_options.Value.DefaultModelPath);
            return _model;
        }
    }
}
