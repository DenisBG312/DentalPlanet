using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalPlanet.Web.ViewModels.User;
using X.PagedList;

namespace DentalPlanet.Web.ViewModels.Dentist
{
    public class DentistIndexViewModel
    {
        public string Id { get; set; }
        public string Specialty { get; set; }
        public string Availability { get; set; }

        public UserViewModel User { get; set; }
        public bool IsDentist { get; set; }
    }
}
