using back_project.DAL;
using back_project.DAL.Entities;
using back_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public EventsController(AppDbContext dbContext)
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

            var eventt = await _dbContext.Events.Include(x => x.EventSpeakers).SingleOrDefaultAsync(t => t.Id == id);

            if (eventt is null) return NotFound();

            List<Speaker> speakers = await _dbContext.Speakers.ToListAsync();

            return View(new EventViewModel
            {
                eventt = eventt,
                speakers = speakers
            });
        }
    }
}
