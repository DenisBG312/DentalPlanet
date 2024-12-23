using DentalPlanet.Data;
using DentalPlanet.Data.Models;
using DentalPlanet.Web.ViewModels.Dentist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentalPlanet.Web.Controllers
{
    public class DentistController : Controller
    {
        private readonly ApplicationDbContext context;
        public DentistController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dentists = await context.Dentists
                .Include(u => u.User)
                .ToListAsync();
            
            return View(dentists);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var availableUsers = await context.Users
                .Where(user => !context.Dentists.Any(dentist => dentist.UserId == user.Id))
                .ToListAsync();

            if (!availableUsers.Any())
            {
                TempData["NoUsers"] = "There are no users left to assign as dentists.";
            }


            ViewData["Users"] = new SelectList(availableUsers, "Id", "UserName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DentistCreateViewModel model)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                bool userAlreadyDentist = await context.Dentists.AnyAsync(d => d.UserId == model.UserId);
                if (userAlreadyDentist)
                {
                    ModelState.AddModelError("UserId", "The selected user is already assigned as a dentist.");
                }
                else
                {
                    var dentist = new Dentist
                    {
                        Specialty = model.Specialty,
                        Availability = model.Availability,
                        UserId = model.UserId
                    };

                    context.Add(dentist);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                var availableUsers = await context.Users
                    .Where(user => !context.Dentists.Any(dentist => dentist.UserId == user.Id))
                    .ToListAsync();
                
                ViewData["Users"] = new SelectList(availableUsers, "Id", "UserName");
                return View(model);
            }

            var users = await context.Users.ToListAsync();
            ViewData["Users"] = new SelectList(users, "Id", "UserName");

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var searchedDentist = await context.Dentists
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (searchedDentist == null)
            {
                return BadRequest();
            }

            return View(searchedDentist);
        }
    }
}
