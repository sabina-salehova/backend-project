namespace back_project.DAL.Entities
{
    public class Event : Entity
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Venue { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ImageName { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }        
    }
}
