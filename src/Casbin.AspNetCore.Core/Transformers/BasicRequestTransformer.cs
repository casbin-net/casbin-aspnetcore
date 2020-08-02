using System.Linq;
using System.Security.Claims;
using Casbin.AspNetCore.Abstractions;

namespace Casbin.AspNetCore.Core.Transformers
{
    public class BasicRequestTransformer : IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

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

        public virtual string ActTransform(ICasbinAuthorizationContext context)
            => context.Data.Action ?? string.Empty;

        public virtual object ObjTransform(ICasbinAuthorizationContext context)
            => context.Data.Resource ?? string.Empty;
    }
}
