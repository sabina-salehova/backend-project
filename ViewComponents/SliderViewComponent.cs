using back_project.DAL;
using back_project.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_project.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public SliderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Slider> sliders = await _dbContext.Sliders.Where(s=>!s.IsDeleted).ToListAsync();

            return View(sliders);
        }
    }
}
