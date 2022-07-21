using System.Threading.Tasks;
using Casbin.Model;
using Casbin.AspNetCore.Authorization.Extensions;
using System;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class KeyMatchRequestTransformer : BasicRequestTransformer
    {
        public override ValueTask<IRequestValues> TransformAsync(ICasbinAuthorizationContext context, IRequestValues data)
        {
            var sub = SubTransform(context, data);
            var obj = ObjTransform(context, data, (c, _) =>
                    c.HttpContext.Request.Path);
            var act = ActTransform(context, data, (c, _) =>
                    c.HttpContext.Request.Method);

            return new ValueTask<IRequestValues>(Request.Create<string, string, string>(sub, obj, act));
        }
    }
}
