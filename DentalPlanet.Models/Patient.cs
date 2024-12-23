using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DentalPlanet.Data.Models
{
    public class Patient
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(200)]
        public string MedicalHistory { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
