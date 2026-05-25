using Microsoft.AspNetCore.Mvc;
using TrainingManagement.MVC.Models;
using TrainingManagement.MVC.Utilities;

namespace TrainingManagement.MVC.Controllers
{
    

    public class CoursesWebController : Controller
    {
        private readonly ITrainingManagementServiceProvider _serviceProvider;
        public CoursesWebController(ITrainingManagementServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<IActionResult> Index()
        {
            string url = "https://localhost:7120/api/courses";
            ResponseDTO responseDto = new ResponseDTO();
            var response = _serviceProvider.SendRequestAsync(new Models.RequestDTO()
            {
                Url = url,
                Method = "get"
            });
            responseDto.Data = response.Result.Data;
            
            return View();
        }
    }
}
