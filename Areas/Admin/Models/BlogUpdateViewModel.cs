using Microsoft.AspNetCore.Mvc.Rendering;

namespace back_project.Areas.Admin.Models
{
    public class BlogUpdateViewModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Writer { get; set; }
        public DateTime Date { get; set; }
        public int CommentCount { get; set; }
        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
