using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalPlanet.Data.Models;
using DentalPlanet.Web.ViewModels.Dentist;

namespace DentalPlanet.Services.Data.Interfaces
{
    public interface IDentistService
    {
        Task<List<Dentist>> GetDentistsAsync();
        Task<List<ApplicationUser>> GetAvailableUsersAsync();
        Task<bool> CreateDentistAsync(DentistCreateViewModel model);
    }
}
