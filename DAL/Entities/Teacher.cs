namespace back_project.DAL.Entities
{
    public class Teacher : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageName { get; set; }
        public string? SkypeAddressName { get; set; }
        public string? FacebookUrl { get; set; }
        public string? PinterestUrl { get; set; }
        public string? VimeoUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public int? LanguageProgress { get; set; }
        public int? TeamLeaderProgress { get; set; }
        public int? DevelopmentProgress { get; set; }
        public int? DesignProgress { get; set; }
        public int? InnovationProgress { get; set; }
        public int? CommunicationProgress { get; set; }
    }
}
