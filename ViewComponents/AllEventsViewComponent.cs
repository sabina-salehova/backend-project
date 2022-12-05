using back_project.DAL;
using back_project.DAL.Entities;
using back_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.ViewComponents
{
    public class AllEventsViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public AllEventsViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Event> events = await _dbContext.Events
                .Where(s => !s.IsDeleted)
                .Include(x => x.EventSpeakers)
                .ToListAsync();

            return View(events);
        }
    }
}
