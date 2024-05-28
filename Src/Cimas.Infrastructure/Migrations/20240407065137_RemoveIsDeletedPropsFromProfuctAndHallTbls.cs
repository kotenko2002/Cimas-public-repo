using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cimas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDeletedPropsFromProfuctAndHallTbls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Halls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Halls",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
