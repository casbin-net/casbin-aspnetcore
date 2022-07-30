using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Tests.Extensions;
using Casbin.AspNetCore.Tests.Fixtures;
using Casbin.AspNetCore.Tests.Utilities;
using Casbin.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class CasbinEvaluatorTest : IClassFixture<TestServerFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICasbinAuthorizationPolicyProvider _casbinPolicyCreator;
        private readonly ICasbinAuthorizationContextFactory<RequestValues<string, string, string, string, string>> _casbinAuthorizationContextFactory;
        private const string DefaultScheme = "context.User";

        public CasbinEvaluatorTest(TestServerFixture testServerFixture)
        {
            _serviceProvider = testServerFixture.TestServer.Services;
            _casbinPolicyCreator = _serviceProvider.GetRequiredService<ICasbinAuthorizationPolicyProvider>();
            _casbinAuthorizationContextFactory = _serviceProvider.GetRequiredService<ICasbinAuthorizationContextFactory<RequestValues<string, string, string, string, string>>>();
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
            var casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);
            var policy = _casbinPolicyCreator.GetAuthorizationPolicy(casbinContext.AuthorizationData);
            var result = AuthenticateResult.Success(new AuthenticationTicket(httpContext.User, DefaultScheme));

            // Act
            var authorizationResult  = await casbinEvaluator.AuthorizeAsync(casbinContext, policy, result);

            // Assert
            Assert.Equal(expectResult, authorizationResult.Succeeded);
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
            var casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action) { Issuer = testIssuer }, httpContext);
            var policy = _casbinPolicyCreator.GetAuthorizationPolicy(casbinContext.AuthorizationData);
            var result = AuthenticateResult.Success(new AuthenticationTicket(httpContext.User, DefaultScheme));

            // Act
            var authorizationResult  = await casbinEvaluator.AuthorizeAsync(casbinContext, policy, result);

            // Assert
            Assert.Equal(expectResult, authorizationResult.Succeeded);
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
            var casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action) { PreferSubClaimType = testClaimType },
                httpContext);
            var policy = _casbinPolicyCreator.GetAuthorizationPolicy(casbinContext.AuthorizationData);
            var result = AuthenticateResult.Success(new AuthenticationTicket(httpContext.User, DefaultScheme));

            // Act
            var authorizationResult  = await casbinEvaluator.AuthorizeAsync(casbinContext, policy, result);

            // Assert
            Assert.Equal(expectResult ,authorizationResult.Succeeded);
        }
    }
}
