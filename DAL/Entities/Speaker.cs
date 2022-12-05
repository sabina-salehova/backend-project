namespace back_project.DAL.Entities
{
    public class Speaker : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string ImageName { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
    }
}
