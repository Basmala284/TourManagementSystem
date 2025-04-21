using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resetPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AvailableSeats",
                table: "TripPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TripCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resetPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvailableSeats",
                table: "TripPackages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TripCategory");
        }
    }
}
