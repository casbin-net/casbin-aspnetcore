using System.Collections.Generic;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class KeyMatchRequestTransformer : BasicRequestTransformer
    {
        public override ValueTask<TRequest> TransformAsync<TRequest>(ICasbinAuthorizationContext<TRequest> context,
            ICasbinAuthorizationData<TRequest> data)
        {
            ref var values = ref data.Values;
            if (values is not RequestValues<string, string, string, string, string> v)
            {
                return new ValueTask<TRequest>(data.Values);
            }

            values.TrySetValue(0, SubTransform(context, data));
            values.TrySetValue(1,  context.HttpContext.Request.Path);
            values.TrySetValue(2, context.HttpContext.Request.Method);
            return new ValueTask<TRequest>(data.Values);
        }
    }
}
