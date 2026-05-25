using Microsoft.AspNetCore.Mvc;

namespace TrainingManagement.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return Problem("An error occurred.");
        }
    }
}
