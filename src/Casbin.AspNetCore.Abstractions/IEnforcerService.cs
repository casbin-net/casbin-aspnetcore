using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public interface IEnforceService
    {
        public Task<bool> EnforceAsync<TRequest>(ICasbinAuthorizationContext<TRequest> context) where TRequest : IRequestValues;
    }
}
