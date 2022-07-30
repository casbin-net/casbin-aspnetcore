using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformer<TRequest> where TRequest : IRequestValues
    {
        public string? Issuer { get; set; }
        public string PreferSubClaimType { get; set; }

        public ValueTask<TRequest> TransformAsync(ICasbinAuthorizationContext<TRequest> context,
            ICasbinAuthorizationData<TRequest> data);
    }
}
