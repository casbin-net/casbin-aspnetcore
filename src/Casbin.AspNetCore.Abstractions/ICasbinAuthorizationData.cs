using System;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinAuthorizationData<TRequest> where TRequest : IRequestValues
    {
        public ref TRequest Values { get; }
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public Type? RequestTransformerType { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}
