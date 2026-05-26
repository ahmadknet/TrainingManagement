using System.Text.Json;
using Newtonsoft.Json;

namespace TrainingManagement.MVC.Models
{
    public class RequestDTO
    {
        public string? Url { get; set; }
        public string? Method { get; set; }
        public object? Data { get; set; }
        public string? AuthToken { get; set; }
    }

    public class UpdateDataRequestDTO : RequestDTO
    {
        public int RecordId { get; set; }
    }
}
