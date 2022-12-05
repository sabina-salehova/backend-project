using Microsoft.AspNetCore.Mvc;

namespace back_project.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
