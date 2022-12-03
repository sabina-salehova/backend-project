namespace back_project.Areas.Admin.Models
{
    public class SliderCreateViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IFormFile Image { get; set; }
        public string ButtonName { get; set; }
        public string ButtonLink { get; set; }
    }
}
