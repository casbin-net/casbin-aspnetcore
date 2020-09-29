using System;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationData
    {
        public string? Value1 { get; }
        public string? Value2 { get; }
        public string? Value3 { get; }
        public string? Value4 { get; }
        public string? Value5 { get; }
        public string[]? CustomValues { get; }
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public Type? RequestTransformerType { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}
