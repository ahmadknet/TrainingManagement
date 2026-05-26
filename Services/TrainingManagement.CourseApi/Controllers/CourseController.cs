using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainingManagement.CourseApi.Data;
using TrainingManagement.CourseApi.Models;

namespace TrainingManagement.CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        //add constructor and inject the dbcontext
        private readonly Models.CourseDbContext _context;
        //create constructor method               
        public CourseController(Models.CourseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ResponseDTO Get()
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
                responseDTO.Errors?.Add(ex.Message);
                responseDTO.Message = "Error occurred while retrieving courses.";
                responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return responseDTO;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] RequestDTO requestDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            var courseDetails = JsonConvert.DeserializeObject<Course[]>(requestDTO.Data?.ToString() ?? "[]");
            if (courseDetails == null || courseDetails.Length == 0)
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Message = "No course data provided.";
                responseDTO.StatusCode = StatusCodes.Status400BadRequest;
                return responseDTO;
            }

            try
            {
                foreach (var c in courseDetails)
                {
                    // ensure an id is set for the new record
                    if (c.CourseId == 0)
                        c.CourseId = new Random().Next(1, int.MaxValue / 2);

                    _context.Courses.Add(c);
                }

                var intRecords = _context.SaveChanges();
                if (intRecords > 0)
                {
                    responseDTO.StatusCode = StatusCodes.Status201Created;
                    responseDTO.Message = "Course(s) added successfully";
                    responseDTO.IsRequestProcessed = true;
                }
                else
                {
                    responseDTO.Message = "Course(s) could not be added";
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            catch (Exception ex)
            {
                responseDTO.Data = null;
                responseDTO.IsRequestProcessed = false;
                responseDTO.Errors?.Add(ex.Message);
                responseDTO.Message = "Error occurred while adding courses. Please try again";
                responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return responseDTO;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] RequestDTO requestDTO)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                var course = JsonConvert.DeserializeObject<Course>(requestDTO.Data?.ToString() ?? string.Empty);
                if (course == null)
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Message = "No course data provided for update.";
                    responseDTO.StatusCode = StatusCodes.Status400BadRequest;
                    return responseDTO;
                }

                var existing = _context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
                if (existing == null)
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Message = "Course not found.";
                    responseDTO.StatusCode = StatusCodes.Status404NotFound;
                    return responseDTO;
                }

                // update fields
                existing.CourseName = course.CourseName;
                existing.CourseDescription = course.CourseDescription;
                existing.Duration = course.Duration;

                var updated = _context.SaveChanges();
                if (updated > 0)
                {
                    responseDTO.IsRequestProcessed = true;
                    responseDTO.Message = "Course updated successfully.";
                    responseDTO.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Message = "No changes were made to the course.";
                    responseDTO.StatusCode = StatusCodes.Status204NoContent;
                }
            }
            catch (Exception ex)
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Errors?.Add(ex.Message);
                responseDTO.Message = "Error occurred while updating course.";
                responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return responseDTO;
        }

        [HttpDelete("{id}")]
        public ResponseDTO Delete(int id)
        {
            var responseDTO = new ResponseDTO();
            try
            {
                var existing = _context.Courses.FirstOrDefault(c => c.CourseId == id);
                if (existing == null)
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Message = "Course not found.";
                    responseDTO.StatusCode = StatusCodes.Status404NotFound;
                    return responseDTO;
                }

                _context.Courses.Remove(existing);
                var deleted = _context.SaveChanges();
                if (deleted > 0)
                {
                    responseDTO.IsRequestProcessed = true;
                    responseDTO.Message = "Course deleted successfully.";
                    responseDTO.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    responseDTO.IsRequestProcessed = false;
                    responseDTO.Message = "Course could not be deleted.";
                    responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            catch (Exception ex)
            {
                responseDTO.IsRequestProcessed = false;
                responseDTO.Errors?.Add(ex.Message);
                responseDTO.Message = "Error occurred while deleting course.";
                responseDTO.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return responseDTO;
        }
    }
}
