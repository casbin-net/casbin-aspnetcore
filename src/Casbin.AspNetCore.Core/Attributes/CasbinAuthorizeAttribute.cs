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
            ValueCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string value1)
        {
            Value1 = value1;
            ValueCount++;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string value1, string value2)
            : this(value1)
        {
            Value2 = value2;
            ValueCount++;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string value1, string value2, string value3)
            : this(value1, value2)
        {
            Value3 = value3;
            ValueCount++;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4)
            : this(value1, value2, value3)
        {
            Value4 = value4;
            ValueCount++;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5)
            : this(value1, value2, value3, value4)
        {
            Value5 = value5;
            ValueCount++;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class. 
        /// </summary>
        public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, params string[] customValues)
            : this(value1, value2, value3, value4, value5)
        {
            CustomValues = customValues;
            ValueCount += CustomValues.Length;
        }

        public string Value1 { get; } = string.Empty;
        public string Value2 { get; } = string.Empty;
        public string Value3 { get; } = string.Empty;
        public string Value4 { get; } = string.Empty;
        public string Value5 { get; } = string.Empty;
        public string[]? CustomValues { get; }
        public int ValueCount { get; }
        public string? Issuer { get; set; }
        public string? PreferSubClaimType { get; set; }
        public Type? RequestTransformerType { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}
