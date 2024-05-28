using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cimas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFilmDurationFieldType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Films",
                type: "nvarchar(48)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Films",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(48)");
        }
    }
}
