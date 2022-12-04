using Microsoft.AspNetCore.Mvc.Rendering;

namespace back_project.Areas.Admin.Models
{
    public class CourseUpdateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string HowToApply { get; set; }
        public string Certification { get; set; }
        public string LeaveAReply { get; set; }
        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
        public DateTime Starts { get; set; }
        public string Duration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesments { get; set; }
        public decimal Fee { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
