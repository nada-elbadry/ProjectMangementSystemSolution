using Microsoft.AspNetCore.Mvc;

namespace ProjectMangementPL.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index(int id)
        {
            return RedirectToRoute("Trainers", new {action ="GetTrainers"});
        }

        public ActionResult GetMembers()
        { 
        return View();
        }
        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
