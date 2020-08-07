using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Transformers;
using Casbin.AspNetCore.Tests.Utilities;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class RequestTransformerTest
    {
        [Fact]
        public async Task ShouldBasicTransform()
        {
            var transformer = new BasicRequestTransformer();

            // Success
            var user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "alice"))
                .Build();
            var casbinContext =  new CasbinAuthorizationContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });

            // Act
            var requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First())).ToArray();

            // Assert
            Assert.Equal("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);

            // Failed
            user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.Role, "alice"))
                .Build();
            casbinContext =  new CasbinAuthorizationContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });

            // Act
            requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First())).ToArray();

            // Assert
            Assert.NotEqual("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);
        }

        [Fact]
        public async Task ShouldBasicTransformWhenSpecSubClaimType()
        {
            const string testClaimType = ClaimTypes.Role;
            var transformer = new BasicRequestTransformer
            {
                PreferSubClaimType = testClaimType
            };

            // Success
            var user = new TestUserBuilder()
                .AddClaim(new Claim(testClaimType, "alice"))
                .Build();
            var casbinContext =  new CasbinAuthorizationContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });

            // Act
            var requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First())).ToArray();

            // Assert
            Assert.Equal("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);

            // Failed
            user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "alice"))
                .Build();
            casbinContext =  new CasbinAuthorizationContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });

            // Act
            requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First())).ToArray();

            // Assert
            Assert.NotEqual("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);
        }

        [Fact]
        public async Task ShouldBasicTransformWhenSpecIssuer()
        {
            const string testIssuer = "LOCAL";
            var transformer = new BasicRequestTransformer
            {
                Issuer = testIssuer
            };

            // Success
            var user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "alice",
                    ClaimValueTypes.String, testIssuer))
                .Build();
            var casbinContext =  new CasbinAuthorizationContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });

            // Act
            var requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First())).ToArray();

            // Assert
            Assert.Equal("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);

            // Failed
            user = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, "alice"))
                .Build();
            casbinContext =  new CasbinAuthorizationContext(user,
                new CasbinAuthorizationData
                {
                    Resource = "data1", Action = "write"
                });

            // Act
            requestValues = (await transformer.TransformAsync(casbinContext, casbinContext.AuthorizationData.First())).ToArray();

            // Assert
            Assert.NotEqual("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);
        }
    }
}
