using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization
{
    public interface IEnforceService
    {
        public Task<bool> EnforceAsync(ICasbinAuthorizationContext context);
    }
}
