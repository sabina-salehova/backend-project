using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_project.Areas.Admin.Models;
using back_project.Areas.Admin.Data;
using back_project.Data;

namespace back_project.Areas.Admin.Controllers
{
    public class TestimonialSlidersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public TestimonialSlidersController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<TestimonialSlider> testimonialSliders = await _dbContext.TestimonialSliders.Where(t => !t.IsDeleted).ToListAsync();
            return View(testimonialSliders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var testimonialSliders = await _dbContext.TestimonialSliders.FindAsync(id);

            if (testimonialSliders is null) return NotFound();

            return View(testimonialSliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialSliderCreateViewModel model)
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

            string unicalName = await model.Image.GenerateFile(Constants.TestimonialSliderPath);

            await _dbContext.TestimonialSliders.AddAsync(new TestimonialSlider
            {
                FullName=model.FullName,
                Position=model.Position,
                Testimonial=model.Testimonial,
                ImageName=unicalName
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var testimonialSliders = await _dbContext.TestimonialSliders.FindAsync(id);

            if (testimonialSliders is null) return NotFound();

            return View(new TestimonialSliderUpdateViewModel
            {
                FullName= testimonialSliders.FullName,
                Position= testimonialSliders.Position,
                Testimonial= testimonialSliders.Testimonial,
                ImageUrl = testimonialSliders.ImageName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TestimonialSliderUpdateViewModel model)
        {
            if (id is null)
                return NotFound();

            var existTestimonialSliders = await _dbContext.TestimonialSliders.FindAsync(id);

            if (existTestimonialSliders is null)
                return NotFound();

            if (existTestimonialSliders.Id != id)
                return BadRequest();

            TestimonialSliderUpdateViewModel testimonialSliderModel = new TestimonialSliderUpdateViewModel
            {
                FullName= existTestimonialSliders.FullName,
                Position= existTestimonialSliders.Position,
                Testimonial= existTestimonialSliders.Testimonial,
                ImageUrl = existTestimonialSliders.ImageName
            };

            if (!ModelState.IsValid)
            {
                return View(testimonialSliderModel);
            }

            var updateImage = existTestimonialSliders.ImageName;

            if (model.Image is not null)
            {
                var newImage = model.Image;

                if (!newImage.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil formati secilmelidir");

                    return View(testimonialSliderModel);
                }
                int imageMbCount = 10;
                if (!newImage.IsAllowedSize(imageMbCount))
                {
                    ModelState.AddModelError("Image", $"Shekilin olcusu {imageMbCount} mb-dan boyuk ola bilmez");

                    return View(testimonialSliderModel);
                }

                var path = Path.Combine(Constants.TestimonialSliderPath, existTestimonialSliders.ImageName);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.TestimonialSliderPath);

                updateImage = unicalFileName;
            }

            existTestimonialSliders.FullName = model.FullName;
            existTestimonialSliders.Position = model.Position;
            existTestimonialSliders.Testimonial = model.Testimonial;
            existTestimonialSliders.ImageName = updateImage;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existTestimonialSliders = await _dbContext.TestimonialSliders.FindAsync(id);

            if (existTestimonialSliders is null) return NotFound();

            var path = Path.Combine(Constants.TestimonialSliderPath, existTestimonialSliders.ImageName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.TestimonialSliders.Remove(existTestimonialSliders);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
