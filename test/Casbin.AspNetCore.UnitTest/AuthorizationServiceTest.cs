using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Policy;
using Casbin.AspNetCore.UnitTest.Fixtures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Casbin.AspNetCore.UnitTest
{
    public class AuthorizationServiceTest : IClassFixture<TestServerFixture>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICasbinAuthorizationContextFactory _casbinAuthorizationContextFactory;

        public AuthorizationServiceTest(TestServerFixture testServerFixture)
        {
            var services = testServerFixture.TestServer.Services;
            _authorizationService = services.GetRequiredService<IAuthorizationService>();
            _casbinAuthorizationContextFactory = services.GetRequiredService<ICasbinAuthorizationContextFactory>();
        }

        [Fact]
        public async Task ShouldBasicAuthorizeAsync()
        {
            // alice
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "alice")
            }));

            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    ResourceName = "data1", ActionName = "write"
                });
            var result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, new CasbinAuthorizationRequirement());
            Assert.False(result.Succeeded);

            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    ResourceName = "data1", ActionName = "read"
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, new CasbinAuthorizationRequirement());
            Assert.True(result.Succeeded);

            // bob
            user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "bob")
            }));

            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    ResourceName = "data2", ActionName = "write"
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, new CasbinAuthorizationRequirement());
            Assert.True(result.Succeeded);

            
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    ResourceName = "data2", ActionName = "read"
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, new CasbinAuthorizationRequirement());
            Assert.False(result.Succeeded);
        }
    }
}
