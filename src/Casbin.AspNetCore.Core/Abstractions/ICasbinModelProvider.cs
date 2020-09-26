using NetCasbin.Model;

// ReSharper disable once CheckNamespace
namespace Casbin.AspNetCore.Authorization
{
    public interface ICasbinModelProvider
    {
        public Model? GetModel();
    }
}
