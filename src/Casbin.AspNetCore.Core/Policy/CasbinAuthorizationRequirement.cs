using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization.Policy
{
    public class CasbinAuthorizationRequirement : IAuthorizationRequirement
    {
        public static CasbinAuthorizationRequirement Requirement { get; }
            = new CasbinAuthorizationRequirement();
    }
}
