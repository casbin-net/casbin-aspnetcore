using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Casbin.AspNetCore.Authorization
{
    public class RequestTransformersCache : IRequestTransformersCache
    {
        private readonly IServiceProvider _serviceProvider;

        public RequestTransformersCache(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Initial();
        }

        public IEnumerable<IRequestTransformer>? Transformers { get; set; }

        private void Initial()
        {
            Transformers ??= _serviceProvider.GetServices<IRequestTransformer>().ToArray();
        }
    }
}
