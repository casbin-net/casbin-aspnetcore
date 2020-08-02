using System.Collections.Generic;

namespace Casbin.AspNetCore.Abstractions
{
    public interface IRequestTransformersCache
    {
        public IEnumerable<IRequestTransformer>? Transformers { get; set; }
    }
}
