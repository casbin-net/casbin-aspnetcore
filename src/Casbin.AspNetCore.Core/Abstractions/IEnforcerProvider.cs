using NetCasbin;

// ReSharper disable once CheckNamespace
namespace Casbin.AspNetCore.Abstractions
{
    public interface IEnforcerProvider
    {
        public Enforcer? GetEnforcer();
    }
}
