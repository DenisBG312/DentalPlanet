using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalPlanet.Data.Models;
using DentalPlanet.Data.Repository.Interfaces;
using DentalPlanet.Services.Data.Interfaces;
using DentalPlanet.Web.ViewModels.Dentist;
using Microsoft.EntityFrameworkCore;

namespace DentalPlanet.Services.Data
{
    public class DentistService : IDentistService
    {
        private readonly IRepository<Dentist, string> _dentistRepository;
        private readonly IRepository<ApplicationUser, string> _userRepository;
        public DentistService(
            IRepository<Dentist, string> dentistRepository,
            IRepository<ApplicationUser, string> userRepository)
        {
            _dentistRepository = dentistRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Dentist>> GetDentistsAsync()
        {
            var dentists = await _dentistRepository
                .GetAllAttached()
                .Include(u => u.User)
                .ToListAsync();

            return dentists;
        }

        public async Task<List<ApplicationUser>> GetAvailableUsersAsync()
        {
            var allUsers = await _userRepository.GetAllAsync();

            var dentistUserIds = await _dentistRepository
                .GetAllAttached()
                .Select(d => d.UserId)
                .ToListAsync();

            return allUsers.Where(user => !dentistUserIds.Contains(user.Id)).ToList();
        }

        public async Task<bool> CreateDentistAsync(DentistCreateViewModel model)
        {
            var isAlreadyDentist = await _dentistRepository
                .GetAllAttached()
                .AnyAsync(d => d.UserId == model.UserId);

            if (isAlreadyDentist)
            {
                return false;
            }

            var dentist = new Dentist
            {
                Specialty = model.Specialty,
                Availability = model.Availability,
                UserId = model.UserId
            };

            await _dentistRepository.AddAsync(dentist);
            await _dentistRepository.SaveChangesAsync();

            return true;
        }
    }
}
