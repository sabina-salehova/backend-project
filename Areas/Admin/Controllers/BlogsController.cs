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
    public class BlogsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly CategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public BlogsController(AppDbContext dbContext, CategoryService categoryService, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _dbContext.Blogs.Where(s => !s.IsDeleted).ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Create()
        {
            var modelForCategories = await _categoryService.GetCategories();

            return View(new BlogCreateViewModel
            {
                Categories=modelForCategories.Categories
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            var modelForCategories = await _categoryService.GetCategories();

            var viewModel = new BlogCreateViewModel
            {
                Categories = modelForCategories.Categories
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

            string unicalName = await model.Image.GenerateFile(Constants.BlogPath);

            var createdCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == model.CategoryId);

            await _dbContext.Blogs.AddAsync(new Blog
            {
                Name=model.Name,
                Content=model.Content,
                CommentCount=model.CommentCount,
                Date=model.Date,
                Writer=model.Writer,
                ImageName = unicalName,
                CategoryId = model.CategoryId,
                Category = createdCategory
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog is null) return NotFound();

            var modelForCategories = await _categoryService.GetCategories();

            return View(new BlogUpdateViewModel
            {
                Name = blog.Name,
                Content=blog.Content,
                Writer=blog.Writer,
                CommentCount=blog.CommentCount,
                Date=blog.Date,
                ImageUrl=blog.ImageName,
                CategoryId = blog.CategoryId,
                Categories = modelForCategories.Categories
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateViewModel model)
        {
            var modelForCategories = await _categoryService.GetCategories();

            if (id is null)
                return NotFound();

            var existBlog = await _dbContext.Blogs.FindAsync(id);

            if (existBlog is null)
                return NotFound();

            if (existBlog.Id != id)
                return BadRequest();

            BlogUpdateViewModel blogModel = new BlogUpdateViewModel
            {
                Name = existBlog.Name,
                Content= existBlog.Content,
                Writer=existBlog.Writer,
                CommentCount=existBlog.CommentCount,
                Date=existBlog.Date,
                ImageUrl = existBlog.ImageName,
                Categories = modelForCategories.Categories,
                CategoryId = existBlog.CategoryId
            };

            if (!ModelState.IsValid)
            {
                return View(blogModel);
            }

            var updateImage = blogModel.ImageUrl;

            if (model.Image is not null)
            {
                var newImage = model.Image;

                if (!newImage.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil formati secilmelidir");

                    return View(blogModel);
                }
                int imageMbCount = 10;
                if (!newImage.IsAllowedSize(imageMbCount))
                {
                    ModelState.AddModelError("Image", $"Shekilin olcusu {imageMbCount} mb-dan boyuk ola bilmez");

                    return View(blogModel);
                }

                var path = Path.Combine(Constants.BlogPath, blogModel.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.BlogPath);

                updateImage = unicalFileName;
            }

            var updateCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == model.CategoryId);


            existBlog.Name = model.Name;
            existBlog.Content = model.Content;
            existBlog.Writer = model.Writer;
            existBlog.CommentCount = model.CommentCount;
            existBlog.Date = model.Date;
            existBlog.ImageName = updateImage;
            existBlog.CategoryId = model.CategoryId;
            existBlog.Category = updateCategory;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var blog = await _dbContext.Blogs
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .SingleOrDefaultAsync();

            if (blog is null) return NotFound();

            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existBlog = await _dbContext.Blogs.FindAsync(id);

            if (existBlog is null) return NotFound();

            var path = Path.Combine(Constants.BlogPath, existBlog.ImageName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Blogs.Remove(existBlog);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
