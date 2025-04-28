namespace CarRental.Models
{
    public class RentedCar
    {
        public int CarId { get; set; }
        public Car Car { get; set; }

        public string RenterId { get; set; }
        public AppUser Renter { get; set; }

        public DateTime RentedAt { get; set; } = DateTime.UtcNow;
    }
}
