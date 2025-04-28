using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]

        public string Model { get; set; }
        [Required]

        public int YearOfMake { get; set; }

        [Required]
        public double PricePerDay { get; set; }

        [Required]
        public string PlateNumber { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? ImageURL { get; set; }
        public string? ImageName { get; set; }

        [Required]
        public string color { get; set; }
        public bool IsRented { get; set; } = false;

    }
    
}
