namespace back_project.Areas.Admin.Models
{
    public class TestimonialSliderUpdateViewModel
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Testimonial { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
        public IFormFile? Image { get; set; }
    }
}
