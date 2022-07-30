using System;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the specified authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<string, string, string, string, string>>
{
    private RequestValues<string, string, string, string, string> _values;
    public CasbinAuthorizeAttribute()
    {
        _values = new RequestValues<string, string, string, string, string>("", "", "", "", "");
    }

    public CasbinAuthorizeAttribute(string value1)
    {
        _values = new RequestValues<string, string, string, string, string>(value1, "", "", "", "");
    }

    public CasbinAuthorizeAttribute(string value1, string value2)
    {
        _values = new RequestValues<string, string, string, string, string>(value1, value2, "", "", "");
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3)
    {
        _values = new RequestValues<string, string, string, string, string>(value1, value2, value3, "", "");
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4)
    {
        _values = new RequestValues<string, string, string, string, string>(value1, value2, value3, value4, "");
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5) : this(value1, value2, value3, value4)
    {
        _values = new RequestValues<string, string, string, string, string>(value1, value2, value3, value4, value5);
    }

    public ref RequestValues<string, string, string, string, string> Values => ref _values;
}
