using System.Text;
using System.Text.Json;
using TrainingManagement.MVC.Models;

namespace TrainingManagement.MVC.Utilities
{
    public class TrainingManagementServiceProvider : ITrainingManagementServiceProvider, ITrainingManagementDataServiceProvider
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Send the request described by requestDTO and return the response wrapped in ResponseDTO
        public async Task<ResponseDTO> SendRequestAsync(RequestDTO requestDTO)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                HttpClient objClient = new HttpClient();
                objClient.DefaultRequestHeaders.Clear();
                objClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (requestDTO.AuthToken != null)
                {
                    objClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", requestDTO.AuthToken);
                    switch (requestDTO.Method.ToLower())
                    {
                        case "get":
                            responseDTO = await DoGet(requestDTO, responseDTO, objClient);
                            break;
                        case "post":
                            responseDTO = await DoPost(requestDTO, responseDTO, objClient);
                            break;
                    }
                }
                else
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Errors.Add("Missing Authorization Token");
                }

                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Errors.Add(ex.Message);
                return responseDTO;
            }

        }

        

        private static async Task<ResponseDTO> DoPost(RequestDTO requestDTO, ResponseDTO responseDTO, HttpClient client)
        {
            JsonContent postContent = JsonContent.Create(requestDTO.Data, options: _jsonOptions);
            var postResponse = await client.PostAsync(requestDTO.Url ?? string.Empty, postContent);
            if(postResponse.IsSuccessStatusCode)
            {
                var content = await postResponse.Content.ReadAsStringAsync();
                responseDTO.StatusCode = (int)postResponse.StatusCode;
                responseDTO.Data = JsonSerializer.Deserialize<object>(content, _jsonOptions);
            }
            else
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Errors.Add($"Request failed with status code: {postResponse.StatusCode}");
            }
            return responseDTO;
        }

        private static async Task<ResponseDTO> DoGet(RequestDTO requestDTO, ResponseDTO responseDTO, HttpClient client)
        {
            var httpResponse = await client.GetAsync(requestDTO.Url ?? string.Empty);
            var content = await httpResponse.Content.ReadAsStringAsync();
            responseDTO.StatusCode = (int)httpResponse.StatusCode;
            responseDTO.Data = JsonSerializer.Deserialize<object>(content, _jsonOptions);
            return responseDTO;
        }

        public async Task<ResponseDTO> SendUpdateRequestAsync(UpdateDataRequestDTO requestDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            HttpClient objClient = new HttpClient();
            objClient.DefaultRequestHeaders.Clear();
            objClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return await DoPut(requestDTO, responseDTO, objClient);
        }

        private static async Task<ResponseDTO> DoPut(UpdateDataRequestDTO requestDTO, ResponseDTO responseDTO, HttpClient objClient)
        {
            JsonContent putContent = JsonContent.Create(requestDTO.Data, options: _jsonOptions);
            var postResponse = await objClient.PostAsync(requestDTO.Url+"/id="+requestDTO.RecordId, putContent);
            if(postResponse.IsSuccessStatusCode)
            {
                var content = await postResponse.Content.ReadAsStringAsync();
                responseDTO.StatusCode = (int)postResponse.StatusCode;
                responseDTO.Data = JsonSerializer.Deserialize<object>(content, _jsonOptions);
            }
            else
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Errors.Add($"Request failed with status code: {postResponse.StatusCode}");
            }
            return responseDTO;
        }



    }
}
