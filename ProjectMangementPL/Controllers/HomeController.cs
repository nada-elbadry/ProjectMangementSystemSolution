using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectMangementPL.Models;

namespace ProjectMangementPL.Controllers
{
    public class HomeController : Controller
    {
        //BaseURL/Home/ Index 
        [NonAction ]
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Trainers()
        {
            var Trainers = new[]
            {
                new { Name = "Alice Johnson", Expertise = "Project Management" },
                new { Name = "Bob Smith", Expertise = "Agile Methodologies" },
                new { Name = "Charlie Brown", Expertise = "Risk Management" }
            };
            return Json(Trainers);
        }

        public RedirectResult Redirect()
        {  
           return Redirect("https://www.example.com");
        }

        public ContentResult Content()
        {
            return Content("<h1>Welcome to the Gym Management Platform!</h1>","text/html");
        }


    }
}
