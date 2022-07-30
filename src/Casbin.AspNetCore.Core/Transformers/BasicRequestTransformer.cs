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

        public override ValueTask<TRequest> TransformAsync<TRequest>(ICasbinAuthorizationContext<TRequest> context,
            ICasbinAuthorizationData<TRequest> data)
        {
            ref var values = ref data.Values;
            if (values is not RequestValues<string, string, string, string, string> v)
            {
                return new ValueTask<TRequest>(data.Values);
            }
            values.TrySetValue(0, SubTransform(context, data));
            values.TrySetValue(1, v.Value1);
            values.TrySetValue(2, v.Value2);
            return new ValueTask<TRequest>(data.Values);
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
