using Microsoft.AspNetCore.Identity;

namespace DentalPlanet.Data.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
