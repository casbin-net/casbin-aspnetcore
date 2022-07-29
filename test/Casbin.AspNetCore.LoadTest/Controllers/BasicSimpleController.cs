using Microsoft.AspNetCore.Mvc;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;


namespace Casbin.AspNetCore.LoadTest.Controllers;

[ApiController]
[Route("BasicSimple")]
public class BasicSimpleController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICasbinAuthorizationContextFactory _casbinAuthorizationContextFactory;
    private readonly IEnforcerProvider _enforcerProvider;
    private readonly CasbinAuthorizationRequirement _requirement = new CasbinAuthorizationRequirement();

    public static IEnumerable<object[]> BasicTestData = new[]
        {
            new object[] {"alice", "data1", "read", true},
            new object[] {"alice", "data1", "write", false},
            new object[] {"bob", "data2", "read", false},
            new object[] {"bob", "data2", "write", true}
        };


    public BasicSimpleController(IAuthorizationService authorizationService, 
        ICasbinAuthorizationContextFactory casbinAuthorizationContextFactory, IEnforcerProvider enforcerProvider)
    {
        _authorizationService = authorizationService;
        _casbinAuthorizationContextFactory = casbinAuthorizationContextFactory;
        _enforcerProvider = enforcerProvider;
    }

    [HttpGet("Verify")]
    public async Task<AuthorizationResult> BasicSimpleGet([FromQuery] string userName, [FromQuery] string resource, [FromQuery] string action)
    {
        var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
            new CasbinAuthorizeAttribute(resource, action), HttpContext);
        return await _authorizationService
            .AuthorizeAsync(HttpContext.User, casbinContext, _requirement);
    }

    [HttpGet("Add")]
    public async Task<bool> BasicSimpleAdd([FromQuery] string userName, [FromQuery] string policy)
    {
        var enforcer = _enforcerProvider.GetEnforcer();
        return await enforcer.AddPolicyAsync(policy.Split(','));
    }

    [HttpGet("Remove")]
    public async Task<bool> BasicSimpleRemove([FromQuery] string userName, [FromQuery] string policy)
    {
        var enforcer = _enforcerProvider.GetEnforcer();
        return await enforcer.RemovePolicyAsync(policy.Split(','));
    }

    [HttpGet("Update")]
    public async Task<bool> BasicSimpleUpdate([FromQuery] string userName, [FromQuery] string oldPolicy, [FromQuery] string newPolicy)
    {
        var enforcer = _enforcerProvider.GetEnforcer();
        return await enforcer.UpdatePolicyAsync(oldPolicy.Split(','), newPolicy.Split(','));
    }
}
