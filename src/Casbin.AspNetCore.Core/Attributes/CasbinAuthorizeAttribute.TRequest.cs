using System;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the specified authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<TRequest> : Attribute, ICasbinAuthorizationData<TRequest> where TRequest : IRequestValues
{
    private TRequest _values;
    /// <summary>
    /// Initializes a new instance of the <see cref="CasbinAuthorizeAttribute"/> class.
    /// </summary>
    public CasbinAuthorizeAttribute(TRequest values)
    {
        _values = values;
    }
    public ref TRequest Values => ref _values;
    public string? Issuer { get; set; }
    public string? PreferSubClaimType { get; set; }
    public Type? RequestTransformerType { get; set; }
    public string? AuthenticationSchemes { get; set; }
}
