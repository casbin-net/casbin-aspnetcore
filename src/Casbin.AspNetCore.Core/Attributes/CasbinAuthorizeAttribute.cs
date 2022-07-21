using System;
using Casbin.Model;

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
        public CasbinAuthorizeAttribute(params string[] values)
        {
            Values = values.Length switch
            {
                1 => Request.Create<string>(values[0]),
                2 => Request.Create<string, string>(values[0], values[1]),
                3 => Request.Create<string, string, string>(values[0], values[1], values[2]),
                4 => Request.Create<string, string, string, string>(
                    values[0], values[1], values[2], values[3]
                    ),
                5 => Request.Create<string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4]
                    ),
                6 => Request.Create<string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5]
                    ),
                7 => Request.Create<string, string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5], values[6]
                    ),
                8 => Request.Create<string, string, string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5], values[6], values[7]),
                9 => Request.Create<string, string, string, string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5], values[6], values[7], values[8]
                    ),
                10 => Request.Create<string, string, string, string, string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5], values[6], values[7], values[8], values[9]
                    ),
                11 => Request.Create<string, string, string, string, string, string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5], values[6], values[7], values[8], values[9], 
                    values[10]
                    ),
                12 => Request.Create<string, string, string, string, string, string, string, string, string, string, string, string>(
                    values[0], values[1], values[2], values[3], values[4], 
                    values[5], values[6], values[7], values[8], values[9], 
                    values[10], values[11]
                    ),
                _ => throw new ArgumentException("Invalid value count to create a request.")
            };
        }

        public IRequestValues? Values { get; }
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public Type? RequestTransformerType { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}
