using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.ViewComponents
{
    public class TeachersViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public TeachersViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Teacher> teachers = await _dbContext.Teachers.Where(s => !s.IsDeleted).ToListAsync();

            return View(teachers);
        }
    }
}
