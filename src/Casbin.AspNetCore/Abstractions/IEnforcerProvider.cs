using NetCasbin;

namespace Casbin.AspNetCore.Abstractions
{
    public interface IEnforcerProvider
    {
        public Enforcer? GetEnforcer();
    }
}
