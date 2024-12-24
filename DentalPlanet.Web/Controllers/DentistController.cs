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

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || await context.Dentists.FirstOrDefaultAsync(u => u.Id == id) == null)
            {
                return BadRequest();
            }

            Dentist dentist = await context.Dentists.FindAsync(id);

            DentistEditViewModel editDentistModel = new DentistEditViewModel()
            {
                Id = dentist.Id,
                Availability = dentist.Availability,
                Specialty = dentist.Specialty
            };

            return View(editDentistModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DentistEditViewModel dentist)
        {
            if (!ModelState.IsValid)
            {
                return View(dentist);
            }

            var existingDentist = await context.Dentists.FindAsync(dentist.Id);
            if (existingDentist == null)
            {
                return NotFound();
            }

            existingDentist.Specialty = dentist.Specialty;
            existingDentist.Availability = dentist.Availability;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || await context.Dentists.FirstOrDefaultAsync(d => d.Id == id) == null)
            {
                return BadRequest();
            }

            var dentist = await context.Dentists.FindAsync(id);
            if (dentist == null)
            {
                return NotFound();
            }

            return View(dentist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dentist = await context.Dentists.FindAsync(id);
            if (dentist == null)
            {
                return NotFound();
            }

            context.Dentists.Remove(dentist);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
