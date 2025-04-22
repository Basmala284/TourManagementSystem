using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPackages_TripCategory_TripCategoryId",
                table: "TripPackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripCategory",
                table: "TripCategory");

            migrationBuilder.RenameTable(
                name: "TripCategory",
                newName: "TripCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripCategories",
                table: "TripCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPackages_TripCategories_TripCategoryId",
                table: "TripPackages",
                column: "TripCategoryId",
                principalTable: "TripCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPackages_TripCategories_TripCategoryId",
                table: "TripPackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripCategories",
                table: "TripCategories");

            migrationBuilder.RenameTable(
                name: "TripCategories",
                newName: "TripCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripCategory",
                table: "TripCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPackages_TripCategory_TripCategoryId",
                table: "TripPackages",
                column: "TripCategoryId",
                principalTable: "TripCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
