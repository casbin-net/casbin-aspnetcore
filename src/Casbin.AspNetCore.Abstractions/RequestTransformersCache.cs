using System;
using System.Collections.Generic;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization
{
    public interface IRequestTransformersCache<TRequest> where TRequest : IRequestValues
    {
        public IReadOnlyList<IRequestTransformer<TRequest>> Transformers { get; }
        public bool TryGetTransformer(Type type, out IRequestTransformer<TRequest>? transformer);
    }
}
