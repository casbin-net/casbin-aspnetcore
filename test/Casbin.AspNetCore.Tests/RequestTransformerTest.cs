﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Transformers;
using Casbin.AspNetCore.Tests.Extensions;
using Casbin.AspNetCore.Tests.Utilities;
using Casbin.Model;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class RequestTransformerTest
    {
        #region Basic request transformer test

        public static IEnumerable<object[]> BasicTransformerTestData = new[]
        {
            new object[] { ClaimTypes.NameIdentifier,
                "alice", "data1", "read",
                "alice", "data1", "read"},

            new object[] { ClaimTypes.Role,
                "alice", "data1", "write",
                string.Empty, "data1", "write" }
        };

        [Theory]
        [MemberData(nameof(BasicTransformerTestData))]
        public async Task ShouldBasicTransform(
            string claim,
            string userName, string resource,
            string action,
            string userNameExpected, string resourceExpected,
            string actionExpected)
        {
            // Arrange
            var transformer = new BasicRequestTransformer();
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build().CreateDefaultHttpContext();
            var casbinContext = new CasbinAuthorizationContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);

            // Act
            IRequestValues requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First().Values));

            // Assert
            Assert.Equal(userNameExpected, requestValues[0]);
            Assert.Equal(resourceExpected, requestValues[1]);
            Assert.Equal(actionExpected, requestValues[2]);
        }

        public static IEnumerable<object[]> BasicTransformerTestDataWithSpecClaim = new[]
        {
            new object[] { ClaimTypes.Role,
                "alice", "data1", "read",
                "alice", "data1", "read"},

            new object[] { ClaimTypes.NameIdentifier,
                "alice", "data1", "write",
                string.Empty, "data1", "write" }
        };

        [Theory]
        [MemberData(nameof(BasicTransformerTestDataWithSpecClaim))]
        public async Task ShouldBasicTransformWhenSpecClaim(
            string claim,
            string userName, string resource,
            string action,
            string userNameExpected, string resourceExpected,
            string actionExpected)
        {
            // Arrange
            const string testClaimType = ClaimTypes.Role;
            var transformer = new BasicRequestTransformer
            {
                PreferSubClaimType = testClaimType
            };
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build().CreateDefaultHttpContext();
            var casbinContext = new CasbinAuthorizationContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);

            // Act
            IRequestValues requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First().Values));

            // Assert
            Assert.Equal(userNameExpected, requestValues[0]);
            Assert.Equal(resourceExpected, requestValues[1]);
            Assert.Equal(actionExpected, requestValues[2]);
        }

        public static IEnumerable<object[]> BasicTransformerTestDataWithSpecIssuer = new[]
        {
            new object[] { "LOCAL", ClaimTypes.NameIdentifier,
                "alice", "data1", "read",
                "alice", "data1", "read"},

            new object[] { "REMOTE", ClaimTypes.NameIdentifier,
                "alice", "data1", "write",
                string.Empty, "data1", "write" }
        };

        [Theory]
        [MemberData(nameof(BasicTransformerTestDataWithSpecIssuer))]
        public async Task ShouldBasicTransformWhenSpecIssuer(
            string issuer, string claim,
            string userName, string resource,
            string action,
            string userNameExpected, string resourceExpected,
            string actionExpected)
        {
            // Arrange
            const string testIssuer = "LOCAL";
            var transformer = new BasicRequestTransformer
            {
                Issuer = testIssuer
            };
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName,
                    ClaimValueTypes.String, issuer))
                .Build().CreateDefaultHttpContext();
            var casbinContext = new CasbinAuthorizationContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);

            // Act
            IRequestValues requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First().Values));

            // Assert
            Assert.Equal(userNameExpected, requestValues[0]);
            Assert.Equal(resourceExpected, requestValues[1]);
            Assert.Equal(actionExpected, requestValues[2]);
        }

        #endregion

        #region Rbac request transformer test

        public static IEnumerable<object[]> RbacTransformerTestData = new[]
        {
            new object[] { ClaimTypes.Role,
                "alice", "data1", "read",
                "alice", "data1", "read"},

            new object[] { ClaimTypes.NameIdentifier,
                "alice", "data1", "write",
                string.Empty, "data1", "write" }
        };

        [Theory]
        [MemberData(nameof(RbacTransformerTestData))]
        public async Task ShouldRbacTransform(
            string claim,
            string userName, string resource,
            string action,
            string userNameExpected, string resourceExpected,
            string actionExpected)
        {
            // Arrange
            var transformer = new RbacRequestTransformer();
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build().CreateDefaultHttpContext();
            var casbinContext = new CasbinAuthorizationContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);

            // Act
            IRequestValues requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First().Values));

            // Assert
            Assert.Equal(userNameExpected, requestValues[0]);
            Assert.Equal(resourceExpected, requestValues[1]);
            Assert.Equal(actionExpected, requestValues[2]);
        }

        public static IEnumerable<object[]> RbacTransformerTestDataWithSpecClaim = new[]
        {
            new object[] { ClaimTypes.Role,
                "alice", "data1", "read",
                string.Empty, "data1", "read"},

            new object[] { ClaimTypes.NameIdentifier,
                "alice", "data1", "write",
                "alice", "data1", "write" }
        };

        [Theory]
        [MemberData(nameof(RbacTransformerTestDataWithSpecClaim))]
        public async Task ShouldRbacTransformWhenSpecClaim(
            string claim,
            string userName, string resource,
            string action,
            string userNameExpected, string resourceExpected,
            string actionExpected)
        {
            // Arrange
            const string testClaimType = ClaimTypes.NameIdentifier;
            var transformer = new RbacRequestTransformer
            {
                PreferSubClaimType = testClaimType
            };
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build().CreateDefaultHttpContext();
            var casbinContext = new CasbinAuthorizationContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);

            // Act
            IRequestValues requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First().Values));

            // Assert
            Assert.Equal(userNameExpected, requestValues[0]);
            Assert.Equal(resourceExpected, requestValues[1]);
            Assert.Equal(actionExpected, requestValues[2]);
        }

        
        public static IEnumerable<object[]> RbacTransformerTestDataWithSpecIssuer = new[]
        {
            new object[] { "LOCAL", ClaimTypes.Role,
                "alice", "data1", "read",
                "alice", "data1", "read"},

            new object[] { "REMOTE", ClaimTypes.Role,
                "alice", "data1", "write",
                string.Empty, "data1", "write" }
        };

        [Theory]
        [MemberData(nameof(RbacTransformerTestDataWithSpecIssuer))]
        public async Task ShouldRbacTransformWhenSpecIssuer(
            string issuer, string claim,
            string userName, string resource,
            string action,
            string userNameExpected, string resourceExpected,
            string actionExpected)
        {
            // Arrange
            const string testIssuer = "LOCAL";
            var transformer = new RbacRequestTransformer
            {
                Issuer = testIssuer
            };
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName,
                    ClaimValueTypes.String, issuer))
                .Build().CreateDefaultHttpContext();
            var casbinContext = new CasbinAuthorizationContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);

            // Act
            IRequestValues requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First().Values));

            // Assert
            Assert.Equal(userNameExpected, requestValues[0]);
            Assert.Equal(resourceExpected, requestValues[1]);
            Assert.Equal(actionExpected, requestValues[2]);
        }

        #endregion
    }
}
