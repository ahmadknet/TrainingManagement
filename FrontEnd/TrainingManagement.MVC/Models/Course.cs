namespace TrainingManagement.MVC.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseDescription { get; set; } = string.Empty;
        public int Duration { get; set; }
    }
}
