using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cimas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSomeFiledsAndAddStatusFiledToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Sessions",
                newName: "StartDateTime");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "Sessions",
                newName: "StartTime");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
