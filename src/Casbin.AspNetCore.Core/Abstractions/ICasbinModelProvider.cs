using NetCasbin.Model;

// ReSharper disable once CheckNamespace
namespace Casbin.AspNetCore.Abstractions
{
    public interface ICasbinModelProvider
    {
        public Model? GetModel();
    }
}
