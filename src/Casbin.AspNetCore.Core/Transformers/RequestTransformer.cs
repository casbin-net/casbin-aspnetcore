using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casbin.AspNetCore.Authorization.Transformers
{
    public class RequestTransformer : IRequestTransformer
    {
        public virtual string? Issuer { get; set; }
        public virtual string? PreferSubClaimType { get; set; }

        public virtual ValueTask<IEnumerable<object>> TransformAsync(ICasbinAuthorizationContext context, ICasbinAuthorizationData data)
        {
            if (data.ValueCount is 0)
            {
                throw new ArgumentException("Value count is invalid.");
            }

            object[]? requestValues = new object[data.ValueCount];
            int requestValuesLength = requestValues.Length;

            if (requestValuesLength > 0)
            {
                requestValues[0] = data.Value1 ?? string.Empty;
            }
            if (requestValuesLength > 1)
            {
                requestValues[1] = data.Value2 ?? string.Empty;
            }
            if (requestValuesLength > 2)
            {
                requestValues[2] = data.Value3 ?? string.Empty;
            }
            if (requestValuesLength > 3)
            {
                requestValues[3] = data.Value4 ?? string.Empty;
            }
            if (requestValuesLength > 4)
            {
                requestValues[4] = data.Value5 ?? string.Empty;
            }
            if (requestValuesLength <= 5)
            {
                return new ValueTask<IEnumerable<object>>(requestValues);
            }

            // Add the custom values
            if (data.CustomValues is null)
            {
                throw new ArgumentException("Value count is invalid.");
            }

            string[]? customValues = data.CustomValues;
            for (int index = 0; index < customValues.Length; index++)
            {
                requestValues[4 + index] = customValues[index];
            }
            return new ValueTask<IEnumerable<object>>(requestValues);
        }
    }
}
