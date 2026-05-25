using System.ComponentModel.DataAnnotations;

namespace TrainingManagement.CourseApi.Data
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public required string CourseDescription { get; set; }
        public int Duration { get; set; }

    }
}
