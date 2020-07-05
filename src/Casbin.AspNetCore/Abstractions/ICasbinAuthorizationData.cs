namespace Casbin.AspNetCore.Abstractions
{
    public interface ICasbinAuthorizationData
    {
        public string? ResourceName { get; set; }
        public string? ActionName { get; set; }
        public string? Issuer { get; set; }
    }
}
