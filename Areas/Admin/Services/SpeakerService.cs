using back_project.Areas.Admin.Models;
using back_project.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace back_project.Areas.Admin.Services
{
    public class SpeakerService
    {
        private readonly AppDbContext _dbContext;

        public SpeakerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EventCreateViewModel> GetCategories()
        {
            var speakers = await _dbContext.Speakers.Where(c => !c.IsDeleted).Include(x => x.EventSpeakers).ThenInclude(x => x.Event).ToListAsync();

            var speakersSelectListItem = new List<SelectListItem>();

            speakers.ForEach(x => speakersSelectListItem.Add(new SelectListItem(x.Name+" "+x.Surname, x.Id.ToString())));

            var model = new EventCreateViewModel
            {
                Speakers = speakersSelectListItem
            };

            return model;
        }
    }
}
