namespace Casbin.AspNetCore.Authorization
{
    public interface IEnforcerProvider
    {
        public IEnforcer? GetEnforcer();
    }
}
