using Casbin.AspNetCore.UnitTest.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NetCasbin;

namespace Casbin.AspNetCore.UnitTest.Fixtures
{
    public class TestServerFixture
    {
        public TestServerFixture()
        {
            var webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureServices(services =>
            {
                services.AddCasbinAuthorizationCore(options =>
                {
                    options.DefaultEnforcerFactory = model => new Enforcer(TestUtility.GetExampleFile("basic_model.conf"), TestUtility.GetExampleFile("basic_policy.csv"));
                });
                // The option is not necessary now.
                //services.AddAuthorizationCore(options =>
                //{
                //    options.AddPolicy("Casbin", builder =>
                //    {
                //        builder.AddRequirements(new CasbinAuthorizationRequirement());
                //    });
                //});
            });
            webHostBuilder.Configure(app =>
            {

            });
            TestServer = new TestServer(webHostBuilder);
        }

        public TestServer TestServer { get; private set; }
    }
}
