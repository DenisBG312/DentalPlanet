using Microsoft.AspNetCore.Mvc;

namespace DentalPlanet.Web.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
