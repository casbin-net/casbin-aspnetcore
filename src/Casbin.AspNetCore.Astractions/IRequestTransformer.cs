using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data);
    }
}
