using back_project.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace back_project.Areas.Admin.Models
{
    public class BlogCreateViewModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Writer { get; set; }
        public DateTime Date { get; set; }
        public int CommentCount { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }

    }
}
