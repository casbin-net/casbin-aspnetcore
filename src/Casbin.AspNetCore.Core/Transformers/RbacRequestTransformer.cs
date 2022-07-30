using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RbacRequestTransformer : BasicRequestTransformer
    {
        public override string PreferSubClaimType { get; set; } = ClaimTypes.Role;

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
    }
}
