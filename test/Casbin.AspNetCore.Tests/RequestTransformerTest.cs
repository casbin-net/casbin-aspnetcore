using System.Security.Claims;
using Casbin.AspNetCore.Abstractions;
using Casbin.AspNetCore.Transformers;
using Casbin.AspNetCore.UnitTest.Fixtures;
using Casbin.AspNetCore.UnitTest.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Casbin.AspNetCore.UnitTest
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
            string sub = transformer.SubTransform(casbinContext);
            object obj = transformer.ObjTransform(casbinContext);
            string act = transformer.ActTransform(casbinContext);

            // Assert
            Assert.Equal("alice", sub);
            Assert.Equal("data1", obj);
            Assert.Equal("write", act);

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
            sub = transformer.SubTransform(casbinContext);
            obj = transformer.ObjTransform(casbinContext);
            act = transformer.ActTransform(casbinContext);

            // Assert
            Assert.NotEqual("alice", sub);
            Assert.Equal("data1", obj);
            Assert.Equal("write", act);
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
            string sub = transformer.SubTransform(casbinContext);
            object obj = transformer.ObjTransform(casbinContext);
            string act = transformer.ActTransform(casbinContext);

            // Assert
            Assert.Equal("alice", sub);
            Assert.Equal("data1", obj);
            Assert.Equal("write", act);

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
            sub = transformer.SubTransform(casbinContext);
            obj = transformer.ObjTransform(casbinContext);
            act = transformer.ActTransform(casbinContext);

            // Assert
            Assert.NotEqual("alice", sub);
            Assert.Equal("data1", obj);
            Assert.Equal("write", act);
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
            string sub = transformer.SubTransform(casbinContext);
            object obj = transformer.ObjTransform(casbinContext);
            string act = transformer.ActTransform(casbinContext);

            // Assert
            Assert.Equal("alice", sub);
            Assert.Equal("data1", obj);
            Assert.Equal("write", act);

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
            sub = transformer.SubTransform(casbinContext);
            obj = transformer.ObjTransform(casbinContext);
            act = transformer.ActTransform(casbinContext);

            // Assert
            Assert.NotEqual("alice", sub);
            Assert.Equal("data1", obj);
            Assert.Equal("write", act);
        }
    }
}
