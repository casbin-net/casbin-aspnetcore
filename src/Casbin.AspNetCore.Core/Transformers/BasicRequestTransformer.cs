using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class BasicRequestTransformer : IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

        public virtual ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            var requestValues = new object[3];
            requestValues[0] = SubTransform(context, data);
            requestValues[1] = ObjTransform(context, data);
            requestValues[2] = ActTransform(context, data);
            return new ValueTask<IEnumerable<object>>(requestValues);
        }

        public virtual string SubTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
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

        public virtual string ObjTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
            => data.Resource ?? string.Empty;

        public virtual string ActTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
            => data.Action ?? string.Empty;

    }
}
