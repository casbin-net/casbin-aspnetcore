using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Casbin.Model;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Performance.Fixtures;
using Casbin.AspNetCore.Performance.Utilities;
using Casbin.AspNetCore.Performance.Extensions;

namespace Casbin.AspNetCore.Performance
{
    [MemoryDiagnoser]
    [BenchmarkCategory("CasbinEvaluator")]
    [SimpleJob(RunStrategy.Throughput, targetCount: 10, runtimeMoniker: RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RunStrategy.Throughput, targetCount: 10, runtimeMoniker: RuntimeMoniker.Net60)]
    public class CasbinEvaluatorBenchmark
    {
        private BenchmarkServerFixture _benchmarkServerFixture;
        private IServiceProvider _serviceProvider;
        private ICasbinAuthorizationPolicyProvider _casbinPolicyCreator;
        private ICasbinAuthorizationContextFactory<StringRequestValues> _casbinAuthorizationContextFactory;
        private ICasbinEvaluator _casbinEvaluator;
        private string _defaultScheme;

        public CasbinEvaluatorBenchmark()
        {

        }

        public IEnumerable<object[]> BasicTestData()
        {
            yield return new object[] { "alice", "data1", "read", true };
            yield return new object[] { "alice", "data1", "write", false };
            yield return new object[] { "bob", "data2", "read", false };
            yield return new object[] { "bob", "data2", "write", true };
        }
        
        public IEnumerable<object[]> BasicTestDataWithSpecIssuer()
        {
            yield return new object[] { "LOCAL", "alice", "data1", "read", true };
            yield return new object[] { "LOCAL", "alice", "data1", "write", false };
            yield return new object[] { "REMOTE", "bob", "data2", "read", false };
            yield return new object[] { "REMOTE", "bob", "data2", "write", false };
        }

        public static IEnumerable<object[]> BasicTestDataWithSpecClaim()
        {
            yield return new object[] { ClaimTypes.Role, "alice", "data1", "read", true };
            yield return new object[] { ClaimTypes.Role, "alice", "data1", "write", false };
            yield return new object[] { ClaimTypes.NameIdentifier, "bob", "data2", "read", false };
            yield return new object[] { ClaimTypes.NameIdentifier, "bob", "data2", "write", false };
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _benchmarkServerFixture = new BenchmarkServerFixture("basic_model.conf", "basic_policy.csv");
            _serviceProvider = _benchmarkServerFixture.WebHost.Services.GetRequiredService<IServiceProvider>();
            _casbinPolicyCreator = _benchmarkServerFixture.WebHost.Services.GetRequiredService<ICasbinAuthorizationPolicyProvider>();
            _casbinAuthorizationContextFactory = _benchmarkServerFixture.WebHost.Services
                .GetRequiredService<ICasbinAuthorizationContextFactory<StringRequestValues>>();
            _casbinEvaluator = _serviceProvider.GetRequiredService<ICasbinEvaluator>();
            _defaultScheme = "context.User";
        }

        [Benchmark]
        [BenchmarkCategory("BasicAuthorize")]
        [ArgumentsSource(nameof(BasicTestData))]
        public async Task BasicAuthorizeAsync(
            string userName, string resource, string action, bool expectResult)
        {
            // Arrange
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, userName))
                .Build().CreateDefaultHttpContext();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action), httpContext);
            var policy = _casbinPolicyCreator.GetAuthorizationPolicy(casbinContext.AuthorizationData);
            var result = AuthenticateResult.Success(new AuthenticationTicket(httpContext.User, _defaultScheme));

            // Act
            var authorizationResult  = await _casbinEvaluator.AuthorizeAsync(casbinContext, policy, result);

            // Assert
            if(expectResult != authorizationResult.Succeeded){
                throw new Exception("Wrong result in benchmark test.");
            }
        }

        [Benchmark]
        [BenchmarkCategory("BasicAuthorizeWhenSpecIssuer")]
        [ArgumentsSource(nameof(BasicTestDataWithSpecIssuer))]
        public async Task BasicAuthorizeWhenSpecIssuerAsync(
            string issuer, string userName, string resource, string action, bool expectResult)
        {
            // Arrange
            const string testIssuer = "LOCAL";
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(ClaimTypes.NameIdentifier, userName,
                ClaimValueTypes.String, issuer))
                .Build().CreateDefaultHttpContext();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action) { Issuer = testIssuer }, httpContext);
            var policy = _casbinPolicyCreator.GetAuthorizationPolicy(casbinContext.AuthorizationData);
            var result = AuthenticateResult.Success(new AuthenticationTicket(httpContext.User, _defaultScheme));

            // Act
            var authorizationResult  = await _casbinEvaluator.AuthorizeAsync(casbinContext, policy, result);

            // Assert
            if(expectResult != authorizationResult.Succeeded){
                throw new Exception("Wrong result in benchmark test.");
            }
        }

        [Benchmark]
        [BenchmarkCategory("BasicAuthorizeWhenSpecSubClaimType")]
        [ArgumentsSource(nameof(BasicTestDataWithSpecClaim))]
        public async Task BasicAuthorizeWhenSpecSubClaimTypeAsync(
            string claim, string userName, string resource, string action, bool expectResult)
        {
            // Arrange
            const string testClaimType = ClaimTypes.Role;
            var httpContext = new TestUserBuilder()
                .AddClaim(new Claim(claim, userName))
                .Build().CreateDefaultHttpContext();
            var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
                new CasbinAuthorizeAttribute(resource, action) { PreferSubClaimType = testClaimType },
                httpContext);
            var policy = _casbinPolicyCreator.GetAuthorizationPolicy(casbinContext.AuthorizationData);
            var result = AuthenticateResult.Success(new AuthenticationTicket(httpContext.User, _defaultScheme));

            // Act
            var authorizationResult  = await _casbinEvaluator.AuthorizeAsync(casbinContext, policy, result);

            // Assert
            if(expectResult != authorizationResult.Succeeded){
                throw new Exception("Wrong result in benchmark test.");
            }
        }
    }
}
