using System.Collections.Generic;
using System.Security.Claims;

namespace Casbin.AspNetCore.Tests.Utilities
{
    public class TestUserBuilder
    {
        private readonly IList<Claim> _claims = new List<Claim>();

        public TestUserBuilder AddClaim(Claim claim)
        {
            _claims.Add(claim);
            return this;
        }

        public ClaimsPrincipal Build()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(_claims));
        }
    }
}
