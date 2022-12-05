using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.ViewComponents
{
    public class HomeEventsViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public HomeEventsViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Event> events = await _dbContext.Events
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return View(events);
        }
    }
}
