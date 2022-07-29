using Microsoft.AspNetCore.Mvc;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;


namespace Casbin.AspNetCore.LoadTest.Controllers;

[ApiController]
[Route("SpecSimple")]
public class SpecSimpleController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICasbinAuthorizationContextFactory _casbinAuthorizationContextFactory;
    private readonly IEnforcerProvider _enforcerProvider;
    private readonly CasbinAuthorizationRequirement _requirement = new CasbinAuthorizationRequirement();

    public static IEnumerable<object[]> BasicTestDataWithSpecIssuer = new[]
        {
            new object[] {"LOCAL" ,"alice", "data1", "read", true},
            new object[] {"LOCAL", "alice", "data1", "write", false},
            new object[] {"REMOTE", "bob", "data2", "read", false},
            new object[] {"REMOTE", "bob", "data2", "write", false}
        };


    public SpecSimpleController(IAuthorizationService authorizationService, 
        ICasbinAuthorizationContextFactory casbinAuthorizationContextFactory, IEnforcerProvider enforcerProvider)
    {
        _authorizationService = authorizationService;
        _casbinAuthorizationContextFactory = casbinAuthorizationContextFactory;
        _enforcerProvider = enforcerProvider;
    }

    [HttpGet(Name = "Verify")]
    public async Task<AuthorizationResult> Get([FromQuery] string userName, [FromQuery] string resource, [FromQuery] string action)
    {
        var casbinContext = _casbinAuthorizationContextFactory.CreateContext(
            new CasbinAuthorizeAttribute(resource, action), HttpContext);
        return await _authorizationService
            .AuthorizeAsync(HttpContext.User, casbinContext, _requirement);
    }

    [HttpGet(Name = "Add")]
    public async Task<bool> Add([FromQuery] string userName, [FromQuery] string policy)
    {
        var enforcer = _enforcerProvider.GetEnforcer();
        return await enforcer.AddPolicyAsync(policy.Split(','));
    }

    [HttpGet(Name = "Remove")]
    public async Task<bool> Remove([FromQuery] string userName, [FromQuery] string policy)
    {
        var enforcer = _enforcerProvider.GetEnforcer();
        return await enforcer.RemovePolicyAsync(policy.Split(','));
    }

    [HttpGet(Name = "Update")]
    public async Task<bool> Update([FromQuery] string userName, [FromQuery] string oldPolicy, [FromQuery] string newPolicy)
    {
        var enforcer = _enforcerProvider.GetEnforcer();
        return await enforcer.UpdatePolicyAsync(oldPolicy.Split(','), newPolicy.Split(','));
    }
}
