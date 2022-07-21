using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization
{
    public interface ICustomizedAuthorizationDataAdapter
    {
        public ValueTask<IEnumerable<object>> ExecuteAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data);
    }
}
