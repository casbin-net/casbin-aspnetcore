using System;
using System.Collections.Generic;
using System.Linq;
using Casbin.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Casbin.AspNetCore.Authorization
{
    public class RequestTransformersCache<TRequest> : IRequestTransformersCache<TRequest>
     where TRequest : IRequestValues
    {
        private readonly IDictionary<Type, IRequestTransformer<TRequest>> _cache = new Dictionary<Type, IRequestTransformer<TRequest>>();
        private readonly List<IRequestTransformer<TRequest>> _transformers;

        public IReadOnlyList<IRequestTransformer<TRequest>> Transformers => _transformers;

        public RequestTransformersCache(IServiceProvider serviceProvider)
        {
            IEnumerable<IRequestTransformer<TRequest>?>? transformers = serviceProvider.GetServices<IRequestTransformer<TRequest>>();
            foreach (var transformer in transformers)
            {
                if (transformer is null)
                {
                    continue;
                }
                _cache[transformer.GetType()] = transformer;
            }
            _transformers = _cache.Values.ToList();
        }

        public bool TryGetTransformer(Type type, out IRequestTransformer<TRequest>? transformer)
        {
            return _cache.TryGetValue(type, out transformer);
        }
    }
}
