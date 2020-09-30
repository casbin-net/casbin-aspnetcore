using Casbin.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationSample.Controllers
{
    [ApiController]
    public class ApiController : Controller
    {

        [HttpGet]
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
