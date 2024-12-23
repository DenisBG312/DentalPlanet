using DentalPlanet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalPlanet.Web.Controllers
{
    public class DentistController : Controller
    {
        private ApplicationDbContext context;
        public DentistController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task <IActionResult> Index()
        {
            var dentists = await context.Dentists
                .Include(u => u.User)
                .ToListAsync();
            
            return View(dentists);
        }
    }
}
