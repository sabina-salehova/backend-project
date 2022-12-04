namespace back_project.DAL.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
