using System;
using Casbin.AspNetCore.Core.Abstractions;

namespace Casbin.AspNetCore.Core
{
    public class CasbinAuthorizationData : ICasbinAuthorizationData
    {
        public string? Resource { get; set; }
        public string? Action { get; set; }
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public Type? RequestTransformerType { get; set; }
    }
}
