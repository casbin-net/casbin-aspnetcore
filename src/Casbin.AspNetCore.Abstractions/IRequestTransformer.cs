using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformer
    {
        public string? Issuer { get; set; }
        public string PreferSubClaimType { get; set; }

        public ValueTask<TRequest> TransformAsync<TRequest>(ICasbinAuthorizationContext<TRequest> context,
            ICasbinAuthorizationData<TRequest> data)
            where TRequest : IRequestValues;
    }
}
