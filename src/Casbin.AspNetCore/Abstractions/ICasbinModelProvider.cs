using NetCasbin.Model;

namespace Casbin.AspNetCore.Abstractions
{
    public interface ICasbinModelProvider
    {
        public Model? GetModel();
    }
}
