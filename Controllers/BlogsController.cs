using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogsController(AppDbContext dbContext)
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

            var blog = await _dbContext.Blogs.Include(x => x.Category).SingleOrDefaultAsync(t => t.Id == id);

            if (blog is null) return NotFound();

            return View(blog);
        }
    }
}
