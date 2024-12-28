using DentalPlanet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DentalPlanet.Web.ViewModels.Appointment;
using Microsoft.EntityFrameworkCore;
using DentalPlanet.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace DentalPlanet.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
            {
                return NotFound("No patient record found for the current user.");
            }

            var appointments = await _context.Appointments
                .Include(u => u.Dentist)
                .ThenInclude(u => u.User)
                .Where(a => a.PatientId == patient.Id)
                .ToListAsync();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string dentistId)
        {
            if (string.IsNullOrEmpty(dentistId))
            {
                return NotFound();
            }

            var model = new AppointmentCreateViewModel()
            {
                DentistId = dentistId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetCurrentUserId();

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                patient = new Patient
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    DateOfBirth = DateTime.Now.AddYears(-18),
                    MedicalHistory = string.Empty
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DentistId = model.DentistId,
                AppointmentDate = model.AppointmentDate,
                Status = Data.Models.Enums.Status.Scheduled,
                CreatedAt = DateTime.Now
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
