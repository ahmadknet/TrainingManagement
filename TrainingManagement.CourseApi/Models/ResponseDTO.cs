namespace TrainingManagement.CourseApi.Models
{
    public class ResponseDTO
    {
        public object? Data { get; set; }
        public bool IsRequestProcessed { get; set; }
        public string? Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
