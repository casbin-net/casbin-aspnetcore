using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Casbin.AspNetCore.Authorization.Policy
{
    public class DefaultCasbinAuthorizationPolicyProvider : ICasbinAuthorizationPolicyProvider
    {
        public DefaultCasbinAuthorizationPolicyProvider(IOptions<CasbinAuthorizationOptions> options)
        {
            if (options is null)
            {
                throw new NullReferenceException(nameof(options));
            }

            string? defaultAuthenticationSchemes = options.Value.DefaultAuthenticationSchemes;
            ICollection<string> authenticationSchemes = new List<string>();
            if (defaultAuthenticationSchemes is not null)
            {
                AddAuthenticationSchemes(authenticationSchemes, defaultAuthenticationSchemes);
            }
            _defaultPolicy = new AuthorizationPolicy(_casbinAuthorizationRequirements, authenticationSchemes);
        }

        private readonly IEnumerable<IAuthorizationRequirement> _casbinAuthorizationRequirements =
            new[] { CasbinAuthorizationRequirement.Requirement };

        private readonly AuthorizationPolicy _defaultPolicy;

        public AuthorizationPolicy GetAuthorizationPolicy(IEnumerable<ICasbinAuthorizationData> authorizationData)
        {
            if (authorizationData is null)
            {
                throw new ArgumentNullException(nameof(authorizationData));
            }

            ICollection<string>? authenticationSchemes = null;
            foreach (var data in authorizationData)
            {
                if (string.IsNullOrWhiteSpace(data.AuthenticationSchemes))
                {
                    continue;
                }

                authenticationSchemes = _defaultPolicy.AuthenticationSchemes as ICollection<string> ??
                                        _defaultPolicy.AuthenticationSchemes.ToList();

                AddAuthenticationSchemes(authenticationSchemes, data.AuthenticationSchemes);
            }

            return authenticationSchemes is not null
                    ? new AuthorizationPolicy(_casbinAuthorizationRequirements, authenticationSchemes)
                    : _defaultPolicy;
        }

        private static void AddAuthenticationSchemes(ICollection<string> authenticationSchemes,
            string authenticationSchemesString)
        {
            string[] authTypesSplit = authenticationSchemesString.Split(',');
            if (authTypesSplit.Length == 0)
            {
                return;
            }

            foreach (var authType in authTypesSplit)
            {
                if (string.IsNullOrWhiteSpace(authType) is false)
                {
                    authenticationSchemes.Add(authType.Trim());
                }
            }
        }
    }
}
