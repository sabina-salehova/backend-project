using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace back_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}