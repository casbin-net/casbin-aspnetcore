using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Policy;
using Casbin.AspNetCore.Tests.Extensions;
using Casbin.AspNetCore.Tests.Fixtures;
using Casbin.AspNetCore.Tests.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class AuthorizationServiceTest : IClassFixture<TestServerFixture>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICasbinAuthorizationContextFactory _casbinAuthorizationContextFactory;
        private readonly CasbinAuthorizationRequirement _requirement = new CasbinAuthorizationRequirement();

        public AuthorizationServiceTest(TestServerFixture servicesFixture)
        {
            var services = servicesFixture.TestServer.Services;
            _authorizationService = services.GetRequiredService<IAuthorizationService>();
            _casbinAuthorizationContextFactory = services.GetRequiredService<ICasbinAuthorizationContextFactory>();
        }

        public static IEnumerable<object[]> BasicTestData = new[]
        {
            new object[] {"alice", "data1", "read", true},
            new object[] {"alice", "data1", "write", false},
            new object[] {"bob", "data2", "read", false},
            new object[] {"bob", "data2", "write", true}
        };

        [Theory]
        [MemberData(nameof(BasicTestData))]
        public async Task ShouldBasicAuthorizeAsync(
            string userName, string resource, string action, bool expectResult)
        {
            // Arrange
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, userName))
                .Build().CreateDefaultHttpContext();

            // Act
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);
            var result = await _authorizationService
                .AuthorizeAsync(httpContext.User, casbinContext, _requirement);

            // Assert
            Assert.Equal(expectResult, result.Succeeded);
        }

        public static IEnumerable<object[]> BasicTestDataWithSpecIssuer = new[]
        {
            new object[] {"LOCAL" ,"alice", "data1", "read", true},
            new object[] {"LOCAL", "alice", "data1", "write", false},
            new object[] {"REMOTE", "bob", "data2", "read", false},
            new object[] {"REMOTE", "bob", "data2", "write", false}
        };

        [Theory]
        [MemberData(nameof(BasicTestDataWithSpecIssuer))]
        public async Task ShouldBasicAuthorizeWhenSpecIssuerAsync(
            string issuer, string userName, string resource, string action, bool expectResult)
        {
            // Arrange
            const string testIssuer = "LOCAL";
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, userName,
                ClaimValueTypes.String, issuer))
                .Build().CreateDefaultHttpContext();

            // Act
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action) { Issuer = testIssuer }, httpContext);
            var result = await _authorizationService
                .AuthorizeAsync(httpContext.User, casbinContext, _requirement);

            // Assert
            Assert.Equal(expectResult, result.Succeeded);
        }

        public static IEnumerable<object[]> BasicTestDataWithSpecClaim = new[]
        {
            new object[] {ClaimTypes.Role ,"alice", "data1", "read", true},
            new object[] {ClaimTypes.Role, "alice", "data1", "write", false},
            new object[] {ClaimTypes.NameIdentifier, "bob", "data2", "read", false},
            new object[] {ClaimTypes.NameIdentifier, "bob", "data2", "write", false}
        };

        [Theory]
        [MemberData(nameof(BasicTestDataWithSpecClaim))]
        public async Task ShouldBasicAuthorizeWhenSpecSubClaimTypeAsync(
            string claim, string userName, string resource, string action, bool expectResult)
        {
            // Arrange
            const string testClaimType = ClaimTypes.Role;
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build().CreateDefaultHttpContext();

            // Assert
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action) { PreferSubClaimType = testClaimType },
                httpContext);
            var result = await _authorizationService
                .AuthorizeAsync(httpContext.User, casbinContext, _requirement);

            // Act
            Assert.Equal(expectResult, result.Succeeded);
        }
    }
}
