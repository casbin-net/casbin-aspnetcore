using System;
using System.Security.Claims;
using Casbin.AspNetCore.Abstractions;

namespace Casbin.AspNetCore
{
    public class CasbinAuthorizationContext : ICasbinAuthorizationContext
    {
        public CasbinAuthorizationContext(ClaimsPrincipal user, ICasbinAuthorizationData data)
        {
            User = user;
            Data = data;
        }

        public CasbinAuthorizationContext(ClaimsPrincipal user, ICasbinAuthorizationData data,
            Type requestTransformerType) : this(user, data)
        {
            RequestTransformerType = requestTransformerType;
        }

        public ClaimsPrincipal User { get; }
        public ICasbinAuthorizationData Data { get; }
        public Type? RequestTransformerType { get; }
    }
}
