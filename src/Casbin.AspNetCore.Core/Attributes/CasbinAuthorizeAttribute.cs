using System;

namespace Casbin.AspNetCore.Authorization
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CasbinAuthorizeAttribute : Attribute, ICasbinAuthorizationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string resource, string action)
        {
            Resource = resource;
            Action = action;
        }

        public string? Resource { get; set; }
        public string? Action { get; set; }
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public Type? RequestTransformerType { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}
