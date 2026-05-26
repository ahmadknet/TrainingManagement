using TrainingManagement.MVC.Models;
using Newtonsoft.Json;

namespace TrainingManagement.MVC.Utilities
{
    //public interface ITrainingManagementServiceProvider<T> where T : class
    //{
    //    bool UpdateRecord(int recordId, T data, string url, string authToken);
    //}

    public interface ITrainingManagementDataServiceProvider
    {
        Task<ResponseDTO> SendUpdateRequestAsync(UpdateDataRequestDTO requestDTO);
    }

    public interface ITrainingManagementServiceProvider
    {
        Task<ResponseDTO> SendRequestAsync(RequestDTO requestDTO);
    }
}
