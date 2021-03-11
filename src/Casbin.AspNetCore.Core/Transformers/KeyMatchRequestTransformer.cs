using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class KeyMatchRequestTransformer : BasicRequestTransformer
    {
        public override ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            object[] requestValues = new object[3];
            requestValues[0] = SubTransform(context, data);

            requestValues[1] = ObjTransform(context, data, (c, _) =>
                    c.HttpContext.Request.Path);
            requestValues[2] = ActTransform(context, data, (c, _) =>
                    c.HttpContext.Request.Method);
            return new ValueTask<IEnumerable<object>>(requestValues);
        }
    }
}
