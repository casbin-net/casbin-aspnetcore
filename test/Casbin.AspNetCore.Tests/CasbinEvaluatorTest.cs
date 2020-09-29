using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Tests.Fixtures;
using Casbin.AspNetCore.Tests.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class CasbinEvaluatorTest : IClassFixture<TestServerFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICasbinPolicyCreator _casbinPolicyCreator;
        private readonly ICasbinAuthorizationContextFactory _casbinAuthorizationContextFactory;
        private const string _defaultScheme = "context.User";

        public CasbinEvaluatorTest(TestServerFixture testServerFixture)
        {
            _serviceProvider = testServerFixture.TestServer.Services;
            _casbinPolicyCreator = _serviceProvider.GetRequiredService<ICasbinPolicyCreator>();
            _casbinAuthorizationContextFactory = _serviceProvider.GetRequiredService<ICasbinAuthorizationContextFactory>();
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
            var user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, userName))
                .Build();
            var casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizeAttribute(resource, action));
            var policy = _casbinPolicyCreator.Create(casbinContext.AuthorizationData);
            var httpContext = new DefaultHttpContext();
            var result = AuthenticateResult.Success(new AuthenticationTicket(user, _defaultScheme));

            // Act
            var authorizationResult  = await casbinEvaluator.AuthorizeAsync(
                policy, result, httpContext, casbinContext, httpContext);

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
            var user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, userName,
                ClaimValueTypes.String, issuer))
                .Build();
            var casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizeAttribute(resource, action)
                {
                    Issuer = testIssuer
                });
            var policy = _casbinPolicyCreator.Create(casbinContext.AuthorizationData);
            var httpContext = new DefaultHttpContext();
            var result = AuthenticateResult.Success(new AuthenticationTicket(user, _defaultScheme));

            // Act
            var authorizationResult  = await casbinEvaluator.AuthorizeAsync(
                policy, result, httpContext, casbinContext, httpContext);

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
            var user = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build();
            var casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(user,
                new CasbinAuthorizeAttribute(resource, action)
                {
                    PreferSubClaimType = testClaimType
                });
            var policy = _casbinPolicyCreator.Create(casbinContext.AuthorizationData);
            var httpContext = new DefaultHttpContext();
            var result = AuthenticateResult.Success(new AuthenticationTicket(user, _defaultScheme));

            // Act
            var authorizationResult  = await casbinEvaluator.AuthorizeAsync(
                policy, result, httpContext, casbinContext, httpContext);

            // Assert
            Assert.Equal(expectResult ,authorizationResult.Succeeded);
        }
    }
}
