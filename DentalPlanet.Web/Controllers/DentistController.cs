using DentalPlanet.Data;
using DentalPlanet.Data.Models;
using DentalPlanet.Services.Data;
using DentalPlanet.Services.Data.Interfaces;
using DentalPlanet.Web.ViewModels.Dentist;
using DentalPlanet.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace DentalPlanet.Web.Controllers
{
    public class DentistController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IDentistService dentistService;
        private const int pageSize = 3;
        public DentistController(ApplicationDbContext context, IDentistService dentistService)
        {
            this.context = context;
            this.dentistService = dentistService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dentists = await context.Dentists
                .Include(d => d.User)
                .ToListAsync();

            var pagedDentists = dentists
                .Select(d => new DentistIndexViewModel
                {
                    Id = d.Id,
                    Specialty = d.Specialty,
                    Availability = d.Availability,
                    IsDentist = d.UserId == currentUserId,
                    User = new UserViewModel
                    {
                        ProfileImageUrl = d.User.ProfileImageUrl,
                        FirstName = d.User.FirstName,
                        LastName = d.User.LastName,
                        Email = d.User.Email
                    }
                })
                .ToPagedList(page, pageSize);

            return View(pagedDentists);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var availableUsers = await dentistService.GetAvailableUsersAsync();

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
                var isCreated = await dentistService.CreateDentistAsync(model);
                if (!isCreated)
                {
                    ModelState.AddModelError("UserId", "The selected user is already assigned as a dentist.");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var availableUsers = await dentistService.GetAvailableUsersAsync();
            ViewData["Users"] = new SelectList(availableUsers, "Id", "UserName");

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

            var dentist = await context.Dentists
                .Include(u => u.User)
                .FirstOrDefaultAsync(d => d.Id == id);

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
            var dentist = await context.Dentists
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

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
