using System.Diagnostics;
using GymMangementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProjectMangementPL.Models;

namespace ProjectMangementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
           _analyticsService = analyticsService;
        }
        //BaseURL/Home/ Index 
        //[NonAction ]
        public ActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();
            // return View();//Return Default [View with Action Name] View For Action
            //return View();//Return Default View For Action With Passing Model Data
           // return View("Hamada");//Return Specific View For Action
           // return View("hamada",Data);//Return Specific View For Action With Passing Model Data
           return View(Data);
        }
        /*
       // public JsonResult Trainers()
       // {
       //     var Trainers = new[]
       //     {
       //         new { Name = "Alice Johnson", Expertise = "Project Management" },
       //         new { Name = "Bob Smith", Expertise = "Agile Methodologies" },
       //         new { Name = "Charlie Brown", Expertise = "Risk Management" }
       //     };
       //     return Json(Trainers);
       // }

       // public RedirectResult Redirect()
       // {  
       //    return Redirect("https://www.teach-anything.com/");
       // }

       // public ContentResult Content()
       // {
       //     return Content("<h1>Welcome to the Gym Management Platform!</h1>","text/html");
       // }

       // public FileResult DownLoadFile()
       // {
        
       // var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","css","site.css");

       //     var FileBytes = System.IO.File.ReadAllBytes(FilePath);
       //     return File(FileBytes,"text/css","DownLoadableSite.css");

       // }

       //public EmptyResult EmptyAction()
       // {
       //     return new EmptyResult();
       // }
        */
    }
}
