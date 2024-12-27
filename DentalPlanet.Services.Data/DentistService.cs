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
        public DentistService(IRepository<Dentist, string> dentistRepository)
        {
            _dentistRepository = dentistRepository;
        }

        public async Task<List<Dentist>> GetDentistsAsync()
        {
            var dentists = await _dentistRepository
                .GetAllAttached()
                .Include(u => u.User)
                .ToListAsync();

            return dentists;
        }

        public async Task<bool> CreateDentistAsync(DentistCreateViewModel model)
        {
            var userAlreadyDentist = await _dentistRepository.GetAllAttached()
                .AnyAsync(d => d.UserId == model.UserId);

            if (userAlreadyDentist)
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

            return true;
        }
    }
}
