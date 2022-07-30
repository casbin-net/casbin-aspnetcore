using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RequestTransformer : IRequestTransformer
    {
        public virtual string? Issuer { get; set; }
        public virtual string PreferSubClaimType { get; set; } = string.Empty;

        public virtual ValueTask<TRequest> TransformAsync<TRequest>(ICasbinAuthorizationContext<TRequest> context, ICasbinAuthorizationData<TRequest> data)
            where TRequest : IRequestValues
        {
            return new ValueTask<TRequest>(data.Values);
        }
    }
}
