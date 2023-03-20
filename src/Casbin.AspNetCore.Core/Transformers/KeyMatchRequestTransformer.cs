using System.Collections.Generic;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class KeyMatchRequestTransformer : BasicRequestTransformer
    {
        public override ValueTask<StringRequestValues> TransformAsync(
            ICasbinAuthorizationContext<StringRequestValues> context,
            ICasbinAuthorizationData<StringRequestValues> data)
        {
            var values = data.Values;
            values.TrySetValue(0, SubTransform(context, data));
            values.TrySetValue(1, context.HttpContext.Request.Path.Value);
            values.TrySetValue(2, context.HttpContext.Request.Method);
            return new ValueTask<StringRequestValues>(values);
        }
    }
}
