using System.Text.Json;

namespace TrainingManagement.MVC.Models
{
    public class ResponseDTO
    {
        public object? Data { get; set; }
        public bool IsRequestProcessed { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }
    }
}
