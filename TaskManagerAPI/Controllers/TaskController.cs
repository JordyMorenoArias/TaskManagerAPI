using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
