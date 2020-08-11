using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization.Policy
{
    public class CasbinPolicyCreator : ICasbinPolicyCreator
    {
        public CasbinPolicyCreator()
        {
            _emptyPolicy = new AuthorizationPolicy(_casbinAuthorizationRequirements, Array.Empty<string>());
        }

        private readonly IEnumerable<IAuthorizationRequirement> _casbinAuthorizationRequirements =
            new []{CasbinAuthorizationRequirement.Requirement};

        private readonly AuthorizationPolicy _emptyPolicy;

        public AuthorizationPolicy Create(IEnumerable<ICasbinAuthorizationData> authorizationData)
        {
            if (authorizationData is null)
            {
                throw new ArgumentNullException(nameof(authorizationData));
            }

            IList<string>? authenticationSchemes = null;
            foreach (var data in authorizationData)
            {
                var authTypesSplit = data.AuthenticationSchemes?.Split(',');
                if (!(authTypesSplit?.Length > 0))
                {
                    return _emptyPolicy;
                }

                authenticationSchemes ??= new List<string>();

                foreach (var authType in authTypesSplit)
                {
                    if (!string.IsNullOrWhiteSpace(authType))
                    {
                        authenticationSchemes.Add(authType.Trim());
                    }
                }
            }

            return authenticationSchemes is null ? _emptyPolicy : new AuthorizationPolicy(_casbinAuthorizationRequirements, authenticationSchemes);
        }
    }
}
