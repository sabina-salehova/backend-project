namespace back_project.Areas.Admin.Models
{
    public class SpeakerUpdateViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
    }
}
