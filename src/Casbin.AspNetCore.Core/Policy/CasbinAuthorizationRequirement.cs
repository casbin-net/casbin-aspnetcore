using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Core.Policy
{
    public class CasbinAuthorizationRequirement : IAuthorizationRequirement
    {
        public static CasbinAuthorizationRequirement Requirement { get; }
            = new CasbinAuthorizationRequirement();
    }
}
