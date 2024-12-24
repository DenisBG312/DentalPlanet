using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalPlanet.Web.ViewModels.Dentist
{
    public class DentistEditViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Specialty { get; set; }

        [MaxLength(15)]
        public string Availability { get; set; }
    }
}
