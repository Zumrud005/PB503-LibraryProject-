using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryProject.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBorrowerProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedYear",
                table: "Borrowers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Borrowers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Borrowers");

            migrationBuilder.AddColumn<int>(
                name: "PublishedYear",
                table: "Borrowers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
