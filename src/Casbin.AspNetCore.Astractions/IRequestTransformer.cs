using System.Collections.Generic;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public IEnumerable<object> Transform(ICasbinAuthorizationContext context);
    }
}
