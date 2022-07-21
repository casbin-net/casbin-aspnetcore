using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Casbin.Model;
using Casbin.AspNetCore.Authorization.Extensions;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class BasicRequestTransformer : RequestTransformer
    {
        public override string PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

        public override ValueTask<IRequestValues> TransformAsync(ICasbinAuthorizationContext context, IRequestValues data)
        {
            if(data.Count < 2)
            {
                throw new ArgumentException("Cannot find enough values for obj and act.");
            }

            var sub = SubTransform(context, data);
            var obj = ObjTransform(context, data, (_, d) => d[0]);
            var act = ActTransform(context, data, (_, d) => d[1]);

            return new ValueTask<IRequestValues>(Request.Create<string, string, string>(sub, obj, act));
        }

        protected virtual string SubTransform(ICasbinAuthorizationContext context, IRequestValues data)
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

        protected virtual string ObjTransform(ICasbinAuthorizationContext context, IRequestValues data,
            Func<ICasbinAuthorizationContext, IRequestValues, string> valueSelector)
            => valueSelector(context, data);

        protected virtual string ActTransform(ICasbinAuthorizationContext context, IRequestValues data, 
            Func<ICasbinAuthorizationContext, IRequestValues, string> valueSelector)
            => valueSelector(context, data);
    }
}
