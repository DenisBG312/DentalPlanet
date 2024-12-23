using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalPlanet.Data.Models
{
    public class Treatment
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; }
        [Required] 
        public string TreatmentName { get; set; } = string.Empty;
        [Required]
        public decimal Cost { get; set; }
    }
}
