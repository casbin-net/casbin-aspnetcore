using System;
using Casbin.Model;

namespace Casbin.AspNetCore.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T>>
{
    private RequestValues<T> _values;
    public CasbinAuthorizeAttribute(T value)
    {
        _values = Request.CreateValues(value);
    }
    public ref RequestValues<T> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2>>
{
    private RequestValues<T1, T2> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2)
    {
        _values = Request.CreateValues(value1, value2);
    }
    public ref RequestValues<T1, T2> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3>>
{
    private RequestValues<T1, T2, T3> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3)
    {
        _values = Request.CreateValues(value1, value2, value3);
    }
    public ref RequestValues<T1, T2, T3> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4>>
{
    private RequestValues<T1, T2, T3, T4> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        _values = Request.CreateValues(value1, value2, value3, value4);
    }
    public ref RequestValues<T1, T2, T3, T4> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5>>
{
    private RequestValues<T1, T2, T3, T4, T5> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5);
    }
    public ref RequestValues<T1, T2, T3, T4, T5> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8, value9);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Values => ref _values;
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CasbinAuthorizeAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : CasbinAuthorizeBaseAttribute, ICasbinAuthorizationData<RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
{
    private RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _values;
    public CasbinAuthorizeAttribute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)
    {
        _values = Request.CreateValues(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14);
    }
    public ref RequestValues<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Values => ref _values;
}
