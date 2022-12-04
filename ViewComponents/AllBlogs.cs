using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.ViewComponents
{
    public class AllBlogs : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public AllBlogs(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Blog> blogs = await _dbContext.Blogs
                .Where(s => !s.IsDeleted)
                .Include(x => x.Category)
                .ToListAsync();

            return View(blogs);
        }
    }
}
