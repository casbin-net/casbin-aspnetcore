using System;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RequestTransformer : IRequestTransformer
    {
        public virtual string? Issuer { get; set; }
        public virtual string PreferSubClaimType { get; set; } = string.Empty;

        public virtual ValueTask<IRequestValues> TransformAsync(ICasbinAuthorizationContext context, IRequestValues data)
        {
            if (data.Count == 0)
            {
                throw new ArgumentException("Value count is invalid.");
            }

            return new ValueTask<IRequestValues>(data);
        }
    }
}
