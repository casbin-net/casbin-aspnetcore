using System;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the specified authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<StringRequestValues>
{
    private StringRequestValues _values;
    public CasbinAuthorizeAttribute()
    {
        _values = StringRequestValues.Empty;
    }

    public CasbinAuthorizeAttribute(string value1)
    {
        _values = new StringRequestValues(value1);
    }

    public CasbinAuthorizeAttribute(string value1, string value2)
    {
        _values = new StringRequestValues(value1, value2);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3)
    {
        _values = new StringRequestValues(value1, value2, value3);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4)
    {
        _values = new StringRequestValues(value1, value2, value3, value4);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5) : this(value1, value2, value3, value4)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6) : this(value1, value2, value3, value4, value5)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7) : this(value1, value2, value3, value4, value5, value6)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8) : this(value1, value2, value3, value4, value5, value6, value7)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9) : this(value1, value2, value3, value4, value5, value6, value7, value8)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8, value9);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10) : this(value1, value2, value3, value4, value5, value6, value7, value8, value9)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11) : this(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12) : this(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13) : this(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13);
    }

    public CasbinAuthorizeAttribute(string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13, string value14) : this(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13)
    {
        _values = new StringRequestValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14);
    }

    public ref StringRequestValues Values => ref _values;
}
