using CarRental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.DBContext
{
    public class Context : IdentityDbContext<AppUser>
    {
        public Context(DbContextOptions options) : base(options)
        {

        }

        protected Context()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RentedCar>()
                .HasKey(rc => new { rc.CarId, rc.RenterId });

            modelBuilder.Entity<RentedCar>()
                .HasOne(rc => rc.Car)
                .WithMany() 
                .HasForeignKey(rc => rc.CarId);

            modelBuilder.Entity<RentedCar>()
                .HasOne(rc => rc.Renter)
                .WithMany()
                .HasForeignKey(rc => rc.RenterId);
        }



        public DbSet<Car> Cars { get; set; }
        public DbSet<RentedCar> RentedCars { get; set; }
    }
}
