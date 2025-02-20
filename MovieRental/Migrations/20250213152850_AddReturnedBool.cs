using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRental.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnedBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Returned",
                table: "Rentals",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Returned",
                table: "Rentals");
        }
    }
}
