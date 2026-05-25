using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingManagement.CourseApi.Models;

namespace TrainingManagement.CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        //add constructor and inject the dbcontext
        private readonly Models.CourseDbContext _context;
        //create constructor method               
        public CourseApiController(Models.CourseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ResponseDTO GetCourses()
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var courses = _context.Courses.ToList();
                responseDTO.Data = courses;
                responseDTO.IsRequestProcessed = true;
                responseDTO.Message = "Courses retrieved successfully";
                responseDTO.StatusCode = StatusCodes.Status200OK;

            }
            catch (Exception ex)
            {
                responseDTO.Data = null;
                responseDTO.IsRequestProcessed = false;
                responseDTO.Message = $"An error occurred while retrieving courses: {ex.Message}";
                responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return responseDTO;
        }
    }
}
