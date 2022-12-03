using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace back_project.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
           
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}