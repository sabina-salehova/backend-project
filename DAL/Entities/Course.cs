namespace back_project.DAL.Entities
{
    public class Course : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string HowToApply { get; set; }
        public string Certification { get; set; }
        public string LeaveAReply { get; set; }
        public string ImageName { get; set; }
        public DateTime Starts { get; set; }
        public string Duration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesments { get; set; }
        public decimal Fee { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
