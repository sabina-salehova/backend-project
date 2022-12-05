using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_project.Areas.Admin.Models;
using back_project.Areas.Admin.Services;
using back_project.Areas.Admin.Data;
using back_project.Data;

namespace back_project.Areas.Admin.Controllers
{
    public class EventsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly SpeakerService _speakerService;
        private readonly IWebHostEnvironment _environment;

        public EventsController(AppDbContext dbContext, SpeakerService speakerService, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
            _speakerService = speakerService;
        }
        public async Task<IActionResult> Index()
        {
            List<Event> events = await _dbContext.Events.Where(s => !s.IsDeleted).Include(x=>x.EventSpeakers).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            var modelForSpeakers = await _speakerService.GetCategories();

            return View(new EventCreateViewModel
            {
                Speakers = modelForSpeakers.Speakers
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            var modelForSpeakers = await _speakerService.GetCategories();

            var viewModel = new EventCreateViewModel
            {
                Speakers = modelForSpeakers.Speakers
            };

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Sekil secmelisiz");
                return View(viewModel);
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("", "Sekil 10mb-den cox ola bilmez");
                return View(viewModel);
            }

            string unicalName = await model.Image.GenerateFile(Constants.SpeakerAndEventPath);

            List<EventSpeaker> createdEventSpeakers = new List<EventSpeaker>();
            Event createdEvent = new Event();

            foreach (var speakerId in model.SpeakersIds)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                {
                    ModelState.AddModelError("", "Spiker movcud deyil..!");
                }
                createdEventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = speakerId
                });
            }

            await _dbContext.Events.AddAsync(new Event
            {
                Name=model.Name,
                Content=model.Content,
                Venue=model.Venue,
                StartTime=model.StartTime,
                EndTime=model.EndTime,
                ImageName = unicalName,
                EventSpeakers=createdEventSpeakers
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var existEvent = await _dbContext.Events
                .Include(x => x.EventSpeakers)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (existEvent is null) return NotFound();

            return View(new EventDetailsViewModel 
            {
                Eventt=existEvent,
                Speakers=await _dbContext.Speakers.ToListAsync()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existEvent = await _dbContext.Events.Include(x=>x.EventSpeakers).Where(x=>x.Id==id).FirstOrDefaultAsync();
            var eventSpeakers = await _dbContext.EventSpeakers.ToListAsync();

            if (existEvent is null) return NotFound();

            var path = Path.Combine(Constants.SpeakerAndEventPath, existEvent.ImageName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);            

            _dbContext.Events.Remove(existEvent);

            await _dbContext.SaveChangesAsync();

            //foreach (var eventSpeaker in eventSpeakers)
            //{
            //    if (eventSpeaker.Event.Id == id)
            //        _dbContext.EventSpeakers.Remove(eventSpeaker);
            //}

            //await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
