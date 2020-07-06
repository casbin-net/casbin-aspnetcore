using System.Threading.Tasks;

namespace Casbin.AspNetCore.Abstractions
{
    public interface IEnforceService
    {
        public Task<bool> EnforceAsync(ICasbinAuthorizationContext context);
    }
}
