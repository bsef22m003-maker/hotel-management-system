using Microsoft.EntityFrameworkCore;
using HotelManagement.Models;

namespace HotelManagement.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        // ✂ Removed: DbSet<RoomType> RoomTypes (RoomType is now a string on Room)
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Staff> Staff { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Guest Configuration
            modelBuilder.Entity<Guest>(entity =>
            {
                entity.HasKey(e => e.GuestID);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
            });

            // ✂ Removed: RoomType entity configuration (no longer a separate table)

            // Room Configuration
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.RoomID);
                entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.RoomType).HasMaxLength(100); // ✅ RoomType is now a plain string column
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                // ✂ Removed: .HasOne(e => e.RoomType).WithMany(...).HasForeignKey(...)
            });

            // Booking Configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingID);
                entity.Property(e => e.TotalPrice).HasPrecision(10, 2);
                entity.Property(e => e.SpecialRequests).HasMaxLength(1000);
                entity.HasOne(e => e.Guest).WithMany(g => g.Bookings).HasForeignKey(e => e.GuestID);
                entity.HasOne(e => e.Room).WithMany(r => r.Bookings).HasForeignKey(e => e.RoomID);
            });

            // Payment Configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentID);
                entity.Property(e => e.Amount).HasPrecision(10, 2);
                entity.Property(e => e.TransactionID).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.HasOne(e => e.Booking).WithMany(b => b.Payments).HasForeignKey(e => e.BookingID);
            });

            // Staff Configuration
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.StaffID);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Position).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Salary).HasPrecision(10, 2);
            });
        }
    }
}