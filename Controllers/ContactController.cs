using Microsoft.AspNetCore.Mvc;

namespace back_project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
