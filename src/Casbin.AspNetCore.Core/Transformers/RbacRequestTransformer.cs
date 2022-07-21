using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Casbin.Model;
using Casbin.AspNetCore.Authorization.Extensions;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RbacRequestTransformer : BasicRequestTransformer
    {
        public override string PreferSubClaimType { get; set; } = ClaimTypes.Role;

        public override ValueTask<IRequestValues> TransformAsync(ICasbinAuthorizationContext context, IRequestValues data)
        {
            var sub = SubTransform(context, data);

            if(data.Count == 3)
            {
                if(data.Count < 2)
                {
                    throw new ArgumentException("Cannot find enough values for obj and act.");
                }
                var obj = ObjTransform(context, data,
                    (_, d) => d[0]);
                var act = ActTransform(context, data,
                    (_, d) => d[1]);
                return new ValueTask<IRequestValues>(Request.Create<string, string, string>(sub, obj, act));
            }
            else if(data.Count == 4)
            {
                if(data.Count < 3)
                {
                    throw new ArgumentException("Cannot find enough values for obj and act.");
                }
                var dom = DomTransform(context, data,
                    (_, d) => d[0]);
                var obj = ObjTransform(context, data,
                    (_, d) => d[1]);
                var act = ActTransform(context, data,
                    (_, d) => d[2]);
                return new ValueTask<IRequestValues>(Request.Create<string, string, string, string>(sub, dom, obj, act));
            }
            else
            {
                throw new ArgumentException("Invalid built-in value count for rbac model.");
            }
        }

        protected virtual string DomTransform(ICasbinAuthorizationContext context, IRequestValues data, 
            Func<ICasbinAuthorizationContext, IRequestValues, string> valueSelector)
        {
            return valueSelector(context, data);
        }
    }
}
