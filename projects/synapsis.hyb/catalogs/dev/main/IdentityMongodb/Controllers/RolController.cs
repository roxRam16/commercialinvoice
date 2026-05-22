using Microsoft.AspNetCore.Mvc;

namespace IdentityMongodb.Controllers
{
    public class RolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
