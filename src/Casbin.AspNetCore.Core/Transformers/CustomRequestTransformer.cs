using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class CustomRequestTransformer : IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

        public virtual ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            object[] requestValues = new object[3];
            requestValues[0] = SubTransform(context, data);
            requestValues[1] = ObjTransform(context, data);
            requestValues[2] = ActTransform(context, data);
            return new ValueTask<IEnumerable<object>>(requestValues);
        }

        public virtual string SubTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            HttpContext httpContext = context.HttpContext;
            Claim? claim;
            if (Issuer is null)
            {
                claim = httpContext.User.FindFirst(PreferSubClaimType);
                return claim is null ? string.Empty : claim.Value;
            }

            claim = httpContext.User.FindAll(PreferSubClaimType).FirstOrDefault(
                c => string.Equals(c.Issuer, Issuer));
            return claim is null ? string.Empty : claim.Value;
        }

        public virtual string ObjTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            var request = context?.HttpContext.Request;
            if (request == null)
            {
                throw new Exception("Missing HttpRequest in CasbinAuthorizationContext");
            }
            return data.Value1 ?? request.Path.Value?.ToLowerInvariant() ?? string.Empty;
        }

        public virtual string ActTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            var request = context?.HttpContext.Request;
            if (request == null)
            {
                throw new Exception("Missing HttpRequest in CasbinAuthorizationContext");
            }
            return data.Value2 ?? request.Method ?? string.Empty;
        }
    }
}
