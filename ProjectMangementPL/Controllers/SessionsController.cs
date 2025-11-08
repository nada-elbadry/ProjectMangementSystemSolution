using Microsoft.AspNetCore.Mvc;

namespace ProjectMangementPL.Controllers
{
    public class SessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
