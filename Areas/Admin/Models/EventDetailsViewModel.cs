using back_project.DAL.Entities;

namespace back_project.Areas.Admin.Models
{
    public class EventDetailsViewModel
    {
        public Event Eventt { get; set;}
        public List<Speaker> Speakers { get; set; }
    }
}
