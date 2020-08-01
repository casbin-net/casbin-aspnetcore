using System;
using Casbin.AspNetCore.Core;
using Microsoft.Extensions.DependencyInjection;
using NetCasbin;
using Xunit;

namespace Casbin.AspNetCore.Tests
{
    public class ServiceCollectionExtensionTest
    {
        [Fact]
        public void ShouldCheckOptions()
        {
            var collection = new ServiceCollection();

            // Success
            collection.AddCasbinAuthorizationCore(options =>
            {
                options.DefaultEnforcerFactory = m => new Enforcer(m);
            });

            // Failed
            Assert.Throws<ArgumentNullException>(() =>
            {
                collection.AddCasbinAuthorizationCore(options =>
                {
                });
            });
        }
    }
}
