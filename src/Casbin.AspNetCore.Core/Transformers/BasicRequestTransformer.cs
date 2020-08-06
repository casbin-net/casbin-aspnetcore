using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class BasicRequestTransformer : IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

        public virtual IEnumerable<object> Transform(ICasbinAuthorizationContext context)
        {
            var requestValues = new object[3];
            requestValues[0] = SubTransform(context);
            requestValues[1] = ObjTransform(context);
            requestValues[2] = ActTransform(context);
            return requestValues;
        }

        public virtual string SubTransform(ICasbinAuthorizationContext context)
        {
            Claim? claim;
            if (Issuer is null)
            {
                claim = context.User.FindFirst(PreferSubClaimType);
                return claim is null ? string.Empty : claim.Value;
            }

            claim = context.User.FindAll(PreferSubClaimType).FirstOrDefault(
                c => string.Equals(c.Issuer, Issuer));
            return claim is null ? string.Empty : claim.Value;
        }

        public virtual string ObjTransform(ICasbinAuthorizationContext context)
            => context.Data.Resource ?? string.Empty;

        public virtual string ActTransform(ICasbinAuthorizationContext context)
            => context.Data.Action ?? string.Empty;
    }
}
