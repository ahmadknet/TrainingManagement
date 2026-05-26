using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainingManagement.MVC.Models;

namespace TrainingManagement.MVC.Utilities
{
    public class TrainingManagementServiceProvider : ITrainingManagementServiceProvider
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Send the request described by requestDTO and return the response wrapped in ResponseDTO
        public async Task<TrainingManagement.MVC.Models.ResponseDTO> SendRequestAsync(TrainingManagement.MVC.Models.RequestDTO requestDTO)
        {
            var responseDTO = new TrainingManagement.MVC.Models.ResponseDTO();

            try
            {
                //content = null;
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if(requestDTO != null ) {
                    if (!string.IsNullOrWhiteSpace(requestDTO.AuthToken))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", requestDTO.AuthToken);
                    }
                }
                else
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Errors.Add("Missing Authorization Token");
                }
                var method = (requestDTO.Method ?? "get").ToLowerInvariant();

                HttpResponseMessage httpResponse;

                switch (method)
                {
                    case "get":
                        httpResponse = await client.GetAsync(requestDTO.Url ?? string.Empty);
                        var content = await httpResponse.Content.ReadAsStringAsync();
                        responseDTO.StatusCode = (int)httpResponse.StatusCode;
                        responseDTO.Data = JsonSerializer.Deserialize<JsonElement>(content, _jsonOptions);
                        break;
                    case "post":
                        {
                            var payload = requestDTO.Data == null ? string.Empty : JsonSerializer.Serialize(requestDTO.Data, _jsonOptions);
                            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
                            httpResponse = await client.PostAsync(requestDTO.Url ?? string.Empty, postContent);
                        }
                        break;
                    case "put":
                        {
                            var payload = requestDTO.Data == null ? string.Empty : JsonSerializer.Serialize(requestDTO.Data, _jsonOptions);
                            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");
                            httpResponse = await client.PutAsync(requestDTO.Url ?? string.Empty, putContent);
                        }
                        break;
                    case "delete":
                        httpResponse = await client.DeleteAsync(requestDTO.Url ?? string.Empty);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported HTTP method: {requestDTO.Method}");
                }

                responseDTO.StatusCode = (int)httpResponse.StatusCode;

                var contentString = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    // parse content into JsonElement so callers can further deserialize as needed
                    if (!string.IsNullOrWhiteSpace(contentString))
                    {
                        responseDTO.Data = JsonSerializer.Deserialize<JsonElement>(contentString, _jsonOptions);
                    }
                    responseDTO.IsRequestProcessed = true;
                    responseDTO.Message = "Request processed successfully.";
                }
                else
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Message = contentString;
                }
            }
            catch (Exception ex)
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Message = ex.Message;
                responseDTO.StatusCode = 500;
            }

            return responseDTO;
        }
    }
}
