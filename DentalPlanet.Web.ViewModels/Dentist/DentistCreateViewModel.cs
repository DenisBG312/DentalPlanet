using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalPlanet.Web.ViewModels.Dentist
{
    public class DentistCreateViewModel
    {
        [Required]
        public string Specialty { get; set; }

        [MaxLength(15)]
        public string Availability { get; set; }

        [Required]
        [Display(Name = "User")] 
        public string UserId { get; set; }

    }
}
