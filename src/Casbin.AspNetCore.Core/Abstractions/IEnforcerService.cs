using System.Threading.Tasks;

namespace Casbin.AspNetCore.Core.Abstractions
{
    public interface IEnforceService
    {
        public Task<bool> EnforceAsync(ICasbinAuthorizationContext context);
    }
}
