using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string PreferSubClaimType { get; set; }
        public ValueTask<IRequestValues> TransformAsync(ICasbinAuthorizationContext context, IRequestValues data);
    }
}
