using System.Linq;
using System.Security.Claims;
using Casbin.AspNetCore.Core;
using Casbin.AspNetCore.Core.Transformers;
using Casbin.AspNetCore.Tests.Utilities;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class RequestTransformerTest
    {
        [Fact]
        public void ShouldBasicTransform()
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
            var requestValues = transformer.Transform(casbinContext).ToArray();

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
            requestValues = transformer.Transform(casbinContext).ToArray();

            // Assert
            Assert.NotEqual("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);
        }

        [Fact]
        public void ShouldBasicTransformWhenSpecSubClaimType()
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
            var requestValues = transformer.Transform(casbinContext).ToArray();

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
            requestValues = transformer.Transform(casbinContext).ToArray();

            // Assert
            Assert.NotEqual("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);
        }

        [Fact]
        public void ShouldBasicTransformWhenSpecIssuer()
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
            var requestValues = transformer.Transform(casbinContext).ToArray();

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
            requestValues = transformer.Transform(casbinContext).ToArray();

            // Assert
            Assert.NotEqual("alice", requestValues[0]);
            Assert.Equal("data1", requestValues[1]);
            Assert.Equal("write", requestValues[2]);
        }
    }
}
