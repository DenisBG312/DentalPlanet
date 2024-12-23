using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalPlanet.Data.Models.Enums;

namespace DentalPlanet.Data.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }
        [Required]
        public string DentistId { get; set; }
        [ForeignKey(nameof(DentistId))]
        public Dentist Dentist { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public Status Status { get; set; } = Status.Scheduled;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
