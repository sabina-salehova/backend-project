namespace back_project.Areas.Admin.Models
{
    public class SpeakerCreateViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }        
        public IFormFile Image { get; set; }
    }
}
