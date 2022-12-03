using Microsoft.AspNetCore.Mvc;

namespace back_project.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
