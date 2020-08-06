using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Policy;
using Casbin.AspNetCore.Tests.Fixtures;
using Casbin.AspNetCore.Tests.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class AuthorizationServiceTest : IClassFixture<TestServerFixture>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICasbinAuthorizationContextFactory _casbinAuthorizationContextFactory;
        private readonly CasbinAuthorizationRequirement _requirement = new CasbinAuthorizationRequirement();

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
            var user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "alice"))
                .Build();

            // Success
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });
            var result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);

            // Failed
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "read"
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.True(result.Succeeded);

            // bob
            user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "bob"))
                .Build();

            // Success
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data2", Action = "write"
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.True(result.Succeeded);

            // Failed
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data2", Action = "read"
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task ShouldBasicAuthorizeWhenSpecIssuerAsync()
        {
            const string testIssuer = "LOCAL";
            // alice
            var user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "alice",
                ClaimValueTypes.String, testIssuer))
                .Build();

            // Success
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write",
                    Issuer = testIssuer
                });
            var result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);

            // Success
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "read",
                    Issuer = testIssuer
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.True(result.Succeeded);

            // bob
            user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "bob"))
                .Build();

            // Failed
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data2", Action = "write",
                    Issuer = testIssuer
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);

            // Failed
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data2", Action = "read",
                    Issuer = testIssuer
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task ShouldBasicAuthorizeWhenSpecSubClaimTypeAsync()
        {
            const string testClaimType = ClaimTypes.Role;
            // alice
            var user = new TestUserBuilder()
                .AddClaim(new Claim(testClaimType, "alice"))
                .Build();

            // Success
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write",
                    PreferSubClaimType = testClaimType
                });
            var result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);

            // Success
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "read",
                    PreferSubClaimType = testClaimType
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.True(result.Succeeded);

            // bob
            user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "bob"))
                .Build();

            // Failed
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data2", Action = "write",
                    PreferSubClaimType = testClaimType
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);

            // Failed
            casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data2", Action = "read",
                    PreferSubClaimType = testClaimType
                });
            result = await _authorizationService
                .AuthorizeAsync(user, casbinContext, _requirement);
            Assert.False(result.Succeeded);
        }
    }
}
