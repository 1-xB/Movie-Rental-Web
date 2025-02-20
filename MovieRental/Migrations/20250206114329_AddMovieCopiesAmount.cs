using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRental.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieCopiesAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCopies",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCopies",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableCopies",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "TotalCopies",
                table: "Movies");
        }
    }
}
