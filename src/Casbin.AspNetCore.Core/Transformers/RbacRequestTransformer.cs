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

        public override ValueTask<StringRequestValues> TransformAsync(
            ICasbinAuthorizationContext<StringRequestValues> context,
            ICasbinAuthorizationData<StringRequestValues> data)
        {
            var values = data.Values;
            string? value1 = values.Value1;
            string? value2 = values.Value2;
            string? value3 = values.Value3;
            values.TrySetValue(0, SubTransform(context, data));
            values.TrySetValue(1, value1);
            values.TrySetValue(2, value2);
            values.TrySetValue(3, value3);
            return new ValueTask<StringRequestValues>(values);
        }
    }
}
