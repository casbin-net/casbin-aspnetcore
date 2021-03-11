using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RbacRequestTransformer : BasicRequestTransformer
    {
        public override string PreferSubClaimType { get; set; } = ClaimTypes.Role;

        public override ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            object[] requestValues = new object[data.ValueCount + 1];
            requestValues[0] = SubTransform(context, data);

            switch (requestValues.Length)
            {
                case 3:
                    requestValues[1] = ObjTransform(context, data,
                        (_, d) => d.Value1);
                    requestValues[2] = ActTransform(context, data,
                        (_, d) => d.Value2);
                    break;
                case 4:
                    requestValues[1] = DomTransform(context, data,
                        (_, d) => d.Value1);
                    requestValues[2] = ObjTransform(context, data,
                        (_, d) => d.Value2);
                    requestValues[3] = ActTransform(context, data,
                        (_, d) => d.Value3);
                    break;
            }

            return new ValueTask<IEnumerable<object>>(requestValues);
        }

        protected virtual string DomTransform(ICasbinAuthorizationContext context, ICasbinAuthorizationData data, Func<ICasbinAuthorizationContext, ICasbinAuthorizationData, string> valueSelector)
        {
            return valueSelector(context, data);
        }
    }
}
