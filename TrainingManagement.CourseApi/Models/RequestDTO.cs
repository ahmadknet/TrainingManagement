namespace TrainingManagement.CourseApi.Models
{
    public class RequestDTO
    {
        public string? Url { get; set; }
        public string? Method { get; set; }
        public object? Data { get; set; }
    }
}
