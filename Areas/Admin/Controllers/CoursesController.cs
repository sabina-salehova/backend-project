using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_project.Areas.Admin.Models;
using back_project.Areas.Admin.Data;
using back_project.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using back_project.Areas.Admin.Services;

namespace back_project.Areas.Admin.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly CategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public CoursesController(AppDbContext dbContext, CategoryService categoryService, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _dbContext.Courses.Where(s => !s.IsDeleted).ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _categoryService.GetCategories();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            var viewModel = await _categoryService.GetCategories();

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

            string unicalName = await model.Image.GenerateFile(Constants.CoursePath);

            var createdCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == model.CategoryId);

            await _dbContext.Courses.AddAsync(new Course
            {
                Name = model.Name,
                Description = model.Description,
                About = model.About,
                HowToApply = model.HowToApply,
                Certification = model.Certification,
                LeaveAReply = model.LeaveAReply,
                ImageName = unicalName,
                Starts = model.Starts,
                Duration = model.Duration,
                SkillLevel = model.SkillLevel,
                Language = model.Language,
                StudentCount = model.StudentCount,
                Assesments = model.Assesments,
                Fee = model.Fee,
                CategoryId = model.CategoryId,
                Category = createdCategory
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var course = await _dbContext.Courses.FindAsync(id);

            if (course is null) return NotFound();

            var modelForCategories = await _categoryService.GetCategories();

            return View(new CourseUpdateViewModel
            {
                Name=course.Name,
                Description=course.Description,
                About=course.About,
                HowToApply=course.HowToApply,
                Certification=course.Certification,
                LeaveAReply=course.LeaveAReply,
                ImageUrl=course.ImageName,
                Starts=course.Starts,
                Duration=course.Duration,
                SkillLevel=course.SkillLevel,
                Language=course.Language,
                StudentCount=course.StudentCount,
                Assesments=course.Assesments,
                Fee=course.Fee,
                CategoryId=course.CategoryId,
                Categories= modelForCategories.Categories,
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateViewModel model)
        {
            var modelForCategories = await _categoryService.GetCategories();

            if (id is null)
                return NotFound();

            var existCourse = await _dbContext.Courses.FindAsync(id);

            if (existCourse is null)
                return NotFound();

            if (existCourse.Id != id)
                return BadRequest();

            CourseUpdateViewModel courseModel = new CourseUpdateViewModel
            { 
                Name=existCourse.Name,
                Description=existCourse.Description,
                About=existCourse.About,
                Assesments=existCourse.Assesments,
                Duration=existCourse.Duration,
                Starts=existCourse.Starts,
                SkillLevel=existCourse.SkillLevel,
                Certification=existCourse.Certification,
                HowToApply=existCourse.HowToApply,
                LeaveAReply=existCourse.LeaveAReply,
                ImageUrl=existCourse.ImageName,
                Language=existCourse.Language,
                StudentCount=existCourse.StudentCount,
                Fee=existCourse.Fee,
                Categories=modelForCategories.Categories,
                CategoryId=existCourse.CategoryId
            };

            if (!ModelState.IsValid)
            {
                return View(courseModel);
            }

            var updateImage = existCourse.ImageName;

            if (model.Image is not null)
            {
                var newImage = model.Image;

                if (!newImage.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil formati secilmelidir");

                    return View(courseModel);
                }
                int imageMbCount = 10;
                if (!newImage.IsAllowedSize(imageMbCount))
                {
                    ModelState.AddModelError("Image", $"Shekilin olcusu {imageMbCount} mb-dan boyuk ola bilmez");

                    return View(courseModel);
                }

                var path = Path.Combine(Constants.CoursePath, existCourse.ImageName);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.CoursePath);

                updateImage = unicalFileName;
            }

            var updateCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == model.CategoryId);


            existCourse.Name = model.Name;
            existCourse.Description = model.Description;
            existCourse.About = model.About;
            existCourse.Duration = model.Duration;
            existCourse.Category = updateCategory;
            existCourse.HowToApply = model.HowToApply;
            existCourse.Certification = model.Certification;
            existCourse.LeaveAReply = model.LeaveAReply;
            existCourse.ImageName = updateImage;
            existCourse.Starts = model.Starts;
            existCourse.LeaveAReply = model.LeaveAReply;
            existCourse.SkillLevel = model.SkillLevel;
            existCourse.Language = model.Language;
            existCourse.StudentCount = model.StudentCount;
            existCourse.Assesments = model.Assesments;
            existCourse.CategoryId = model.CategoryId;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var course = await _dbContext.Courses
                .Where(x=>x.Id==id)
                .Include(x=>x.Category)
                .SingleOrDefaultAsync();

            if (course is null) return NotFound();

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existCourse = await _dbContext.Courses.FindAsync(id);

            if (existCourse is null) return NotFound();

            var path = Path.Combine(Constants.CoursePath, existCourse.ImageName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Courses.Remove(existCourse);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
