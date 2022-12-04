using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_project.Areas.Admin.Models;
using back_project.Areas.Admin.Data;
using back_project.Data;
using NuGet.DependencyResolver;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;

namespace back_project.Areas.Admin.Controllers
{
    public class TeachersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public TeachersController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Teacher> teachers = await _dbContext.Teachers
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
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

            string unicalName = await model.Image.GenerateFile(Constants.TeacherPath);

            if (model.LanguageProgress > 100 || model.TeamLeaderProgress > 100
                || model.TeamLeaderProgress > 100 || model.DevelopmentProgress > 100
                || model.DesignProgress > 100 || model.CommunicationProgress > 100 || model.InnovationProgress > 100)
            {
                ModelState.AddModelError("", "progress 100-e qeder daxil edilmelidir");
                return View(model);
            }

            await _dbContext.Teachers.AddAsync(new Teacher
            {
                Name=model.Name,
                Surname=model.Surname,
                Position=model.Position,
                About=model.About,
                Degree=model.Degree,
                Experience=model.Experience,
                Hobbies=model.Hobbies,
                Faculty=model.Faculty,
                Email=model.Email,
                Phone=model.Phone,
                ImageName = unicalName,
                SkypeAddressName=model.SkypeAddressName,
                FacebookUrl=model.FacebookUrl,
                PinterestUrl=model.PinterestUrl,
                VimeoUrl=model.VimeoUrl,
                TwitterUrl=model.TwitterUrl,
                LanguageProgress=model.LanguageProgress,
                TeamLeaderProgress=model.TeamLeaderProgress,
                DevelopmentProgress=model.DevelopmentProgress,
                DesignProgress=model.DesignProgress,
                InnovationProgress=model.InnovationProgress,
                CommunicationProgress=model.CommunicationProgress
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var teacher = await _dbContext.Teachers.FindAsync(id);

            if (teacher is null) return NotFound();

            return View(teacher);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var teacher = await _dbContext.Teachers.FindAsync(id);

            if (teacher is null) return NotFound();

            return View(new TeacherUpdateViewModel
            {
                Name = teacher.Name,
                Surname = teacher.Surname,
                Position = teacher.Position,
                About = teacher.About,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Email = teacher.Email,
                Phone = teacher.Phone,
                ImageUrl= teacher.ImageName,
                SkypeAddressName = teacher.SkypeAddressName,
                FacebookUrl = teacher.FacebookUrl,
                PinterestUrl = teacher.PinterestUrl,
                VimeoUrl = teacher.VimeoUrl,
                TwitterUrl = teacher.TwitterUrl,
                LanguageProgress = teacher.LanguageProgress,
                TeamLeaderProgress = teacher.TeamLeaderProgress,
                DevelopmentProgress = teacher.DevelopmentProgress,
                DesignProgress = teacher.DesignProgress,
                InnovationProgress = teacher.InnovationProgress,
                CommunicationProgress = teacher.CommunicationProgress
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateViewModel model)
        {
            if (id is null)
                return NotFound();

            var existTeacher = await _dbContext.Teachers.FindAsync(id);

            if (existTeacher is null)
                return NotFound();

            if (existTeacher.Id != id)
                return BadRequest();

            TeacherUpdateViewModel teacherModel = new TeacherUpdateViewModel
            {
                Name = existTeacher.Name,
                Surname = existTeacher.Surname,
                Position = existTeacher.Position,
                About = existTeacher.About,
                Degree = existTeacher.Degree,
                Experience = existTeacher.Experience,
                Hobbies = existTeacher.Hobbies,
                Faculty = existTeacher.Faculty,
                Email = existTeacher.Email,
                Phone = existTeacher.Phone,
                ImageUrl = existTeacher.ImageName,
                SkypeAddressName = existTeacher.SkypeAddressName,
                FacebookUrl = existTeacher.FacebookUrl,
                PinterestUrl = existTeacher.PinterestUrl,
                VimeoUrl = existTeacher.VimeoUrl,
                TwitterUrl = existTeacher.TwitterUrl,
                LanguageProgress = existTeacher.LanguageProgress,
                TeamLeaderProgress = existTeacher.TeamLeaderProgress,
                DevelopmentProgress = existTeacher.DevelopmentProgress,
                DesignProgress = existTeacher.DesignProgress,
                InnovationProgress = existTeacher.InnovationProgress,
                CommunicationProgress = existTeacher.CommunicationProgress
            };

            if (model.LanguageProgress > 100 || model.TeamLeaderProgress > 100
                || model.TeamLeaderProgress > 100 || model.DevelopmentProgress > 100
                || model.DesignProgress > 100 || model.CommunicationProgress > 100 || model.InnovationProgress > 100)
            {
                ModelState.AddModelError("", "progress 100-e qeder daxil edilmelidir");
                return View(teacherModel);
            }

            if (!ModelState.IsValid)
            {
                return View(teacherModel);
            }

            var updateImage = existTeacher.ImageName;

            if (model.Image is not null)
            {
                var newImage = model.Image;

                if (!newImage.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil formati secilmelidir");

                    return View(teacherModel);
                }
                int imageMbCount = 10;
                if (!newImage.IsAllowedSize(imageMbCount))
                {
                    ModelState.AddModelError("Image", $"Shekilin olcusu {imageMbCount} mb-dan boyuk ola bilmez");

                    return View(teacherModel);
                }

                var path = Path.Combine(Constants.TeacherPath, existTeacher.ImageName);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.TeacherPath);

                updateImage = unicalFileName;
            }

            existTeacher.Name = model.Name;
            existTeacher.Surname = model.Surname;
            existTeacher.Position = model.Position;
            existTeacher.About = model.About;
            existTeacher.Degree = model.Degree;
            existTeacher.Experience = model.Experience;
            existTeacher.Hobbies = model.Hobbies;
            existTeacher.Faculty = model.Faculty;
            existTeacher.Email = model.Email;
            existTeacher.Phone = model.Phone;
            existTeacher.ImageName = updateImage;
            existTeacher.SkypeAddressName = model.SkypeAddressName;
            existTeacher.FacebookUrl = model.FacebookUrl;
            existTeacher.PinterestUrl = model.PinterestUrl;
            existTeacher.VimeoUrl = model.VimeoUrl;
            existTeacher.TwitterUrl = model.TwitterUrl;
            existTeacher.LanguageProgress = model.LanguageProgress;
            existTeacher.TeamLeaderProgress = model.TeamLeaderProgress;
            existTeacher.DevelopmentProgress = model.DevelopmentProgress;
            existTeacher.DesignProgress = model.DesignProgress;
            existTeacher.InnovationProgress = model.InnovationProgress;
            existTeacher.CommunicationProgress = model.CommunicationProgress;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existTeacher = await _dbContext.Teachers.FindAsync(id);

            if (existTeacher is null) return NotFound();

            var path = Path.Combine(Constants.TeacherPath, existTeacher.ImageName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Teachers.Remove(existTeacher);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}
