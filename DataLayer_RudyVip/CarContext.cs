using Console_App_RudyVip;
using Console_App_RudyVip.ObjectClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;

namespace DataLayer_RudyVip
{
    public class CarContext : DbContext
    {
        public DbSet<Car> CarData { get; set; }
        public DbSet<Reservation> ReservationData { get; set; }
        public DbSet<Customer> CustomerData { get; set; }
        public DbSet<ReservationCars> ReservationCarsData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=WILLIAM-SLABBAE\SQLEXPRESS;Initial Catalog=Rudy_Cars;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReservationCars>().HasKey(c => new {c.carID,c.reservationID });
        }
        public CarContext() { }
    }
}
