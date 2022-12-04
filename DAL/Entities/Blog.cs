namespace back_project.DAL.Entities
{
    public class Blog : Entity
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Writer { get; set; }
        public DateTime Date { get; set; }
        public int CommentCount { get; set; }
        public string ImageName { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
