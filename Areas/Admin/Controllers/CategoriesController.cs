using back_project.DAL.Entities;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_project.Areas.Admin.Models;
using back_project.Areas.Admin.Data;

namespace back_project.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public CategoriesController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _dbContext.Categories.Where(s => !s.IsDeleted).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            Category existCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == model.Name);

            if (existCategory is not null)
            {
                ModelState.AddModelError("", "bu adli category artiq movcuddur");
                return View();
            }

            await _dbContext.Categories.AddAsync(new Category
            {
                Name = model.Name
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null) return NotFound();

            return View(new CategoryUpdateViewModel
            {
                Name = category.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateViewModel model)
        {
            if (id is null)
                return NotFound();

            var existCategory = await _dbContext.Categories.FindAsync(id);

            if (existCategory is null)
                return NotFound();

            if (existCategory.Id != id)
                return BadRequest();

            Category modelName = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == model.Name);

            if (modelName is not null)
            {
                ModelState.AddModelError("", "bu adli category artiq movcuddur");
                return View(model);
            }

            CategoryUpdateViewModel categoryModel = new CategoryUpdateViewModel
            {
                Name = existCategory.Name
            };

            if (!ModelState.IsValid)
            {
                return View(categoryModel);
            }

            existCategory.Name = model.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existCategory = await _dbContext.Categories.FindAsync(id);

            if (existCategory is null) return NotFound();

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (course is not null)
            {
                ModelState.AddModelError("", "bu adli category artiq movcuddur");
                return View(id);
            }

            _dbContext.Categories.Remove(existCategory);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
