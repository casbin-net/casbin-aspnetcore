using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class BasicRequestTransformer : RequestTransformer
    {
        public override string? PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

        public override ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            object[] requestValues = new object[data.ValueCount + 1];
            requestValues[0] = SubTransform(context, data);
            requestValues[1] = ObjTransform(context, data, d => d.Value1);
            requestValues[2] = ActTransform(context, data, d => d.Value2);
            return new ValueTask<IEnumerable<object>>(requestValues);
        }

        protected virtual string SubTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
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

        protected virtual string ObjTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data, Func<ICasbinAuthorizationData, string> valueSelector)
            => valueSelector(data);

        protected virtual string ActTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data, Func<ICasbinAuthorizationData, string> valueSelector)
            => valueSelector(data);
    }
}
