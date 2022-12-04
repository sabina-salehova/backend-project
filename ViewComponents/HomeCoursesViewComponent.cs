using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.ViewComponents
{
    public class HomeCoursesViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public HomeCoursesViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Course> courses = await _dbContext.Courses
                .Where(s => !s.IsDeleted)
                .Include(x => x.Category)
                .Take(3)
                .ToListAsync();

            return View(courses);
        }
    }
}
