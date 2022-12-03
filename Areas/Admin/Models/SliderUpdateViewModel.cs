namespace back_project.Areas.Admin.Models
{
    public class SliderUpdateViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IFormFile? Image { get; set; }
        public string ButtonName { get; set; }
        public string ButtonLink { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
    }
}
