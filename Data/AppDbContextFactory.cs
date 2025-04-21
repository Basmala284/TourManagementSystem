using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TourManagementSystem.Data;

namespace TourManagementSystem.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Replace this with your actual connection string
            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-54FG73O\\SQLEXPRESS;Database=TourManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
