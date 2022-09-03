using System.Diagnostics;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Transformers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
using WebApplicationSample.Models;

namespace BlazorWebAssemblySample.Server.Controllers
{
    [Route("Casbin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("AttributeTest")]
        [CasbinAuthorize]
        public string AttribRouteTest(string tenantId)
        {
            return "You have passed the authentication test.";
        }

        [HttpGet("BasicTest")]
        [CasbinAuthorize(nameof(BasicTest), nameof(System.Net.Http.HttpMethod.Get))]
        public string BasicTest()
        {
            return "You have passed the authentication test.";
        }
    }
}
