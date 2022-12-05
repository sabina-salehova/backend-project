using back_project.DAL.Entities;

namespace back_project.Models
{
    public class EventViewModel
    {
        public Event eventt { get; set; }=new Event();
        public List<Speaker> speakers { get; set; } = new List<Speaker>();
    }
}
