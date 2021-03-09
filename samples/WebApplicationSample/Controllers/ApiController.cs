using Casbin.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationSample.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ApiController : Controller
    {

        [HttpGet("index")]
        [CasbinAuthorize]
        public IActionResult Index()
        {
            return new JsonResult(new
            {
                Message = "You passed the casbin authorize."
            });
        }
    }
}
