using NetCasbin.Model;

namespace Casbin.AspNetCore.Core.Abstractions
{
    public interface ICasbinModelProvider
    {
        public Model? GetModel();
    }
}
