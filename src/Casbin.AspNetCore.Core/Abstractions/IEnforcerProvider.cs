using NetCasbin;

namespace Casbin.AspNetCore.Core.Abstractions
{
    public interface IEnforcerProvider
    {
        public Enforcer? GetEnforcer();
    }
}
