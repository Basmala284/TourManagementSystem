using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TripCategory",
                table: "TripPackages");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "FullName");

            migrationBuilder.AddColumn<int>(
                name: "TripCategoryId",
                table: "TripPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TripCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripPackages_TripCategoryId",
                table: "TripPackages",
                column: "TripCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPackages_TripCategory_TripCategoryId",
                table: "TripPackages",
                column: "TripCategoryId",
                principalTable: "TripCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPackages_TripCategory_TripCategoryId",
                table: "TripPackages");

            migrationBuilder.DropTable(
                name: "TripCategory");

            migrationBuilder.DropIndex(
                name: "IX_TripPackages_TripCategoryId",
                table: "TripPackages");

            migrationBuilder.DropColumn(
                name: "TripCategoryId",
                table: "TripPackages");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TripCategory",
                table: "TripPackages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
