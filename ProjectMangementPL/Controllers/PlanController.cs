using Microsoft.AspNetCore.Mvc;

namespace ProjectMangementPL.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
