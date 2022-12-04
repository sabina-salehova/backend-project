using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CoursesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            var course = await _dbContext.Courses.Include(x=>x.Category).SingleOrDefaultAsync(t => t.Id == id);

            if (course is null) return NotFound();

            return View(course);
        }
    }
}
