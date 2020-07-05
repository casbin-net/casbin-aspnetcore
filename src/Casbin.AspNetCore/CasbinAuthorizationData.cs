using Casbin.AspNetCore.Abstractions;

namespace Casbin.AspNetCore
{
    public class CasbinAuthorizationData : ICasbinAuthorizationData
    {
        public string? ResourceName { get; set; }
        public string? ActionName { get; set; }
        public string? Issuer { get; set; }
    }
}
