using System.Threading.Tasks;

namespace TrainingManagement.MVC.Utilities
{
    public interface ITrainingManagementServiceProvider
    {
        Task<TrainingManagement.MVC.Models.ResponseDTO> SendRequestAsync(TrainingManagement.MVC.Models.RequestDTO requestDTO);
    }
}
