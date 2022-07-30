using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RequestTransformer : IRequestTransformer<StringRequestValues>
    {
        public virtual string? Issuer { get; set; }
        public virtual string PreferSubClaimType { get; set; } = string.Empty;

        public virtual ValueTask<StringRequestValues> TransformAsync(
            ICasbinAuthorizationContext<StringRequestValues> context,
            ICasbinAuthorizationData<StringRequestValues> data)
        {
            return new ValueTask<StringRequestValues>(data.Values);
        }
    }
}
