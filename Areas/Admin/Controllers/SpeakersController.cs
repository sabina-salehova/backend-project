using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_project.Areas.Admin.Models;
using back_project.Areas.Admin.Data;
using back_project.Data;

namespace back_project.Areas.Admin.Controllers
{
    public class SpeakersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public SpeakersController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Speaker> speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            return View(speakers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Sekil secmelisiz");
                return View(model);
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("", "Sekil 10mb-den cox ola bilmez");
                return View(model);
            }

            string unicalName = await model.Image.GenerateFile(Constants.SpeakerAndEventPath);

            await _dbContext.Speakers.AddAsync(new Speaker
            {
                Name=model.Name,
                Surname=model.Surname,
                Position=model.Position,
                ImageName = unicalName
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var speaker = await _dbContext.Speakers.FindAsync(id);

            if (speaker is null) return NotFound();

            return View(speaker);
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var speaker = await _dbContext.Speakers.FindAsync(id);

            if (speaker is null) return NotFound();

            return View(new SpeakerUpdateViewModel
            {
                Name= speaker.Name,
                Surname=speaker.Surname,
                Position=speaker.Position,
                ImageUrl = speaker.ImageName
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateViewModel model)
        {
            if (id is null)
                return NotFound();

            var existSpeaker = await _dbContext.Speakers.FindAsync(id);

            if (existSpeaker is null)
                return NotFound();

            if (existSpeaker.Id != id)
                return BadRequest();

            SpeakerUpdateViewModel spekaerModel = new SpeakerUpdateViewModel
            {
                Name= existSpeaker.Name,
                Surname= existSpeaker.Surname,
                Position= existSpeaker.Position,
                ImageUrl = existSpeaker.ImageName
            };

            if (!ModelState.IsValid)
            {
                return View(spekaerModel);
            }

            var updateImage = existSpeaker.ImageName;

            if (model.Image is not null)
            {
                var newImage = model.Image;

                if (!newImage.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil formati secilmelidir");

                    return View(spekaerModel);
                }
                int imageMbCount = 10;
                if (!newImage.IsAllowedSize(imageMbCount))
                {
                    ModelState.AddModelError("Image", $"Shekilin olcusu {imageMbCount} mb-dan boyuk ola bilmez");

                    return View(spekaerModel);
                }

                var path = Path.Combine(Constants.SpeakerAndEventPath, existSpeaker.ImageName);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.SpeakerAndEventPath);

                updateImage = unicalFileName;
            }

            existSpeaker.Name = model.Name;
            existSpeaker.Surname = model.Surname;
            existSpeaker.Position = model.Position;
            existSpeaker.ImageName = updateImage;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existSpeaker = await _dbContext.Speakers.FindAsync(id);

            if (existSpeaker is null) return NotFound();

            var path = Path.Combine(Constants.SpeakerAndEventPath, existSpeaker.ImageName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Speakers.Remove(existSpeaker);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

