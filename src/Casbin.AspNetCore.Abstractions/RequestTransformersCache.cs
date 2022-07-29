using System.Collections.Generic;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformersCache
    {
        public IEnumerable<IRequestTransformer>? Transformers { get; set; }
    }
}
