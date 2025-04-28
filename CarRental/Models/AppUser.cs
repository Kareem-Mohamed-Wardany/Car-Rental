using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Models
{
    public class AppUser : IdentityUser
    {
        public string Address { get; set; }
        public int SSN { get; set; }
        public bool HasRentedCar { get; set; } = false;


    }
}
