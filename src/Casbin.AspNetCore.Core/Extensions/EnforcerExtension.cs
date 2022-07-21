using System;
using System.Threading.Tasks;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization.Extensions
{
    public static class EnforcerExtension
    {
        public static async Task<bool> EnforceFromAuthorizationDataAsync(this IEnforcer enforcer, IRequestValues requestValues)
        {
            return requestValues.Count switch
            {
                1 => await enforcer.EnforceAsync<string>(requestValues[0]),
                2 => await enforcer.EnforceAsync<string, string>(requestValues[0], requestValues[1]),
                3 => await enforcer.EnforceAsync<string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2]
                    ),
                4 => await enforcer.EnforceAsync<string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2], requestValues[3]
                    ),
                5 => await enforcer.EnforceAsync<string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2],
                    requestValues[3], requestValues[4]
                    ),
                6 => await enforcer.EnforceAsync<string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2],
                    requestValues[3], requestValues[4], requestValues[5]
                    ),
                7 => await enforcer.EnforceAsync<string, string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2], requestValues[3],
                    requestValues[4], requestValues[5], requestValues[6]
                    ),
                8 => await enforcer.EnforceAsync<string, string, string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2], requestValues[3],
                    requestValues[4], requestValues[5], requestValues[6], requestValues[7]
                    ),
                9 => await enforcer.EnforceAsync<string, string, string, string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2],
                    requestValues[3], requestValues[4], requestValues[5],
                    requestValues[6], requestValues[7], requestValues[8]
                    ),
                10 => await enforcer.EnforceAsync<string, string, string, string, string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2], requestValues[3],
                    requestValues[4], requestValues[5], requestValues[6], requestValues[7],
                    requestValues[8], requestValues[9]
                    ),
                11 => await enforcer.EnforceAsync<string, string, string, string, string, string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2], requestValues[3],
                    requestValues[4], requestValues[5], requestValues[6], requestValues[7],
                    requestValues[8], requestValues[9], requestValues[10]
                    ),
                12 => await enforcer.EnforceAsync<string, string, string, string, string, string, string, string, string, string, string, string>(
                    requestValues[0], requestValues[1], requestValues[2], requestValues[3],
                    requestValues[4], requestValues[5], requestValues[6], requestValues[7],
                    requestValues[8], requestValues[9], requestValues[10], requestValues[11]
                    ),
                _ => throw new ArgumentException("Invalid request value count.")
            };
        }
    }
}