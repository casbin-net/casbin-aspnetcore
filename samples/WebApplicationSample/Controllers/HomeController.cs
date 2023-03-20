using System.Diagnostics;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Transformers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
using WebApplicationSample.Models;

namespace WebApplicationSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("AttributeTest")]
        [CasbinAuthorize(RequestTransformerType = typeof(KeyMatchRequestTransformer))]
        public IActionResult AttributeTest(string tenantId)
        {
            return View();
        }

        [CasbinAuthorize(nameof(BasicTest), nameof(HttpMethod.Get), RequestTransformerType = typeof(BasicRequestTransformer))]
        public IActionResult BasicTest()
        {
            return View();
        }

        [CasbinAuthorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
