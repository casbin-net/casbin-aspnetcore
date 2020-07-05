

using System;
using System.Linq;
using System.Security.Claims;
using Casbin.AspNetCore.Abstractions;

namespace Casbin.AspNetCore.Transformers
{
    public class BasicRequestTransformer : IRequestTransformer
    {
        public BasicRequestTransformer()
        {
            SubTransformer = context => context.User.FindFirst(ClaimTypes.Name).Value;
            ObjTransformer = context => context.Data.ResourceName ?? string.Empty;
            ActTransformer = context => context.Data.ActionName ?? string.Empty;
        }

        public BasicRequestTransformer(string? issuer) : this()
        {
            if (issuer is null)
            {
                return;
            }

            Issuer = issuer;
            SubTransformer = context =>
            {
                return context.User.FindAll(ClaimTypes.Name).FirstOrDefault(
                    c => string.Equals(c.Issuer, Issuer)).Value;
            };
        }

        public string? Issuer { get; }
        public Func<ICasbinAuthorizationContext, string> SubTransformer { get; set; }
        public Func<ICasbinAuthorizationContext, object> ObjTransformer { get; set; }
        public Func<ICasbinAuthorizationContext, string> ActTransformer { get; set; }
    }
}
