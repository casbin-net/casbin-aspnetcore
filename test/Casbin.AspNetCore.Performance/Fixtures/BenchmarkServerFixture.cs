using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Performance.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Casbin;
using Casbin.Model;
using System;

namespace Casbin.AspNetCore.Performance.Fixtures
{
    public class BenchmarkServerFixture
    {
        public IWebHost WebHost { get; }
        public BenchmarkServerFixture(string confFile, string policyFile){
            var webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureServices(services =>
            {
                services.AddCasbinAuthorization(options =>
                {
                    options.DefaultModelPath = ConfigurationUtility.GetExampleFile(confFile);
                    options.DefaultPolicyPath = ConfigurationUtility.GetExampleFile(policyFile);
                })
                .BuildServiceProvider();
            });
            webHostBuilder.Configure(app =>
            {
                app.UseCasbinAuthorization();
            });
            WebHost = webHostBuilder.Build();
        }
    }
}
