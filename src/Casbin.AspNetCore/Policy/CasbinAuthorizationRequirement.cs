using System;
using Casbin.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Policy
{
    public class CasbinAuthorizationRequirement : IAuthorizationRequirement
    {
        public IRequestTransformer? DefaultRequestTransformer { get; set; }
    }
}
