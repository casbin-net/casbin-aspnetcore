using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.Model;
using Microsoft.AspNetCore.Http;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class BasicRequestTransformer : RequestTransformer
    {
        public override string PreferSubClaimType { get; set; } = ClaimTypes.NameIdentifier;

        public override ValueTask<StringRequestValues> TransformAsync(
            ICasbinAuthorizationContext<StringRequestValues> context,
            ICasbinAuthorizationData<StringRequestValues> data)
        {
            var values = data.Values;
            string? value1 = values.Value1;
            string? value2 = values.Value2;
            values.TrySetValue(0, SubTransform(context, data));
            values.TrySetValue(1, value1);
            values.TrySetValue(2, value2);
            return new ValueTask<StringRequestValues>(values);
        }

        protected virtual string SubTransform<TRequest>(ICasbinAuthorizationContext<TRequest> context, ICasbinAuthorizationData<TRequest> data)
            where TRequest : IRequestValues
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
    }
}
