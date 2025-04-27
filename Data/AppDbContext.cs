using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Numerics;

namespace TourManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public AppDbContext()
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TripPackage> TripPackages { get; set; }
        public DbSet<TravelAgency> TravelAgencies { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TripCategory> TripCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Tourist", NormalizedName = "TOURIST" },
                new IdentityRole { Id="3", Name = "TravelAgency", NormalizedName = "TRAVELAGENCY" }
            };

        
        // Prevent cascade delete on Booking → Tourist
        modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tourist)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.TouristId)
                .OnDelete(DeleteBehavior.Restrict); // 👈 this disables cascade delete

            // Optional: prevent review → booking cascade loops too
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.Id)
                .OnDelete(DeleteBehavior.Restrict);


            // User -> TravelAgency (1-to-M)
            modelBuilder.Entity<TravelAgency>()
               .HasOne(ta => ta.User)               // A TravelAgency has one User
               .WithMany(u => u.TravelAgencies)     // A User can have many TravelAgencies
               .HasForeignKey(ta => ta.UserID);     // Foreign Key in TravelAgency table

            // TravelAgency -> TripPackage (1-to-M)
            modelBuilder.Entity<TripPackage>()
                .HasOne(tp => tp.TravelAgency)      // one Agency Has many tripPackages
                .WithMany(ta => ta.TripPackages)
                .HasForeignKey(tp => tp.TravelAgencyId);

            // TripPackage -> Booking (1-to-M)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.TripPackage)
                .WithMany(tp => tp.Bookings)
                .HasForeignKey(b => b.TourPackageId);

            // User -> Booking (1-to-M)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tourist)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.TouristId);
            // TripPackage -> Review (1-to-M)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.TripPackage)
                .WithMany(tp => tp.Reviews)
                .HasForeignKey(r => r.TourPackageId);

            // User -> Complaints (1-to-M)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Tourist)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.TouristId);

            // Configure One-to-Many relationship between TripCategory and TripPackage
            modelBuilder.Entity<TripPackage>()
                .HasOne(tp => tp.TripCategory)
                .WithMany(tc => tc.TripPackages)
                .HasForeignKey(tp => tp.TripCategoryId);
        }


    }
}
