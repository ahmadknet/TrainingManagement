using System.Text.Json;

namespace TrainingManagement.MVC.Models
{
    public class RequestDTO
    {
        public string? Url { get; set; }
        public string? Method { get; set; }
        public object? Data { get; set; }
        
    }

    //public class ResponseDTO
    //{
    //    public object? Data { get; set; }
    //    public bool IsRequestProcessed { get; set; }
    //    public string? Message { get; set; } = string.Empty;
    //    public int StatusCode { get; set; }
    //}
}
