using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class CarToAdd
    {
        [Required]
        public string Brand { get; set; }
        [Required]

        public string Model { get; set; }
        [Required]

        public int YearOfMake { get; set; }
        [Required]

        public double PricePerDay { get; set; }
        [Required]
        public string color { get; set; }

        [Required]
        public string PlateNumber { get; set; }

        public IFormFile? ImageURL { get; set; }
        public string? ImageName { get; set; }
    }
}
