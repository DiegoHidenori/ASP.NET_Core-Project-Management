using COMP2139_Labs.Models;
using COMP2139_Labs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;

namespace COMP2139_Labs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly ISessionService _sessionService; // Week 14

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //_sessionService = sessionService;
        }


        // Week 14
        public IActionResult Index()
        {
            //const string sessionKey = "VisitCount";
            //int visitCount = _sessionService.GetSessionData<int>(sessionKey);
            //visitCount++;
            //_sessionService.SetSessionData(sessionKey, visitCount);

            //ViewData["VisitCount"] = visitCount;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        // Lab 5 - Project or ProjectTask Search
        // General search action
        [HttpGet]
        public IActionResult GeneralSearch(string searchType, string searchString)
        {
            if (searchType == "Projects")
            {
                return RedirectToAction("Search", "Projects", new { area = "ProjectManagement", searchString });
            }
            else if (searchType == "Tasks")
            {
                //int defaultProjectId = 1;
                //return RedirectToAction("Search", "Tasks", new { projectId = defaultProjectId, searchString, area = "ProjectManagement" });
                var url = Url.Action("Search", "Task", new { area = "ProjectManagement" }) + $"?searchString={searchString}";
                return Redirect(url);
            }
            return RedirectToAction("Index", "Home");
        }


        // Lab 5 - NotFound() Action added
        public IActionResult NotFound(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            return View("Error");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
