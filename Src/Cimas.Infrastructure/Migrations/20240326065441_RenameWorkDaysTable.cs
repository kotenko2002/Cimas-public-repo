using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cimas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameWorkDaysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_WorkDays_WorkDayId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDays_AspNetUsers_UserId",
                table: "WorkDays");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDays_Cinemas_CinemaId",
                table: "WorkDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDays",
                table: "WorkDays");

            migrationBuilder.RenameTable(
                name: "WorkDays",
                newName: "Workdays");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDays_UserId",
                table: "Workdays",
                newName: "IX_Workdays_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDays_CinemaId",
                table: "Workdays",
                newName: "IX_Workdays_CinemaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workdays",
                table: "Workdays",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Workdays_WorkDayId",
                table: "Reports",
                column: "WorkDayId",
                principalTable: "Workdays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workdays_AspNetUsers_UserId",
                table: "Workdays",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workdays_Cinemas_CinemaId",
                table: "Workdays",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Workdays_WorkDayId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Workdays_AspNetUsers_UserId",
                table: "Workdays");

            migrationBuilder.DropForeignKey(
                name: "FK_Workdays_Cinemas_CinemaId",
                table: "Workdays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workdays",
                table: "Workdays");

            migrationBuilder.RenameTable(
                name: "Workdays",
                newName: "WorkDays");

            migrationBuilder.RenameIndex(
                name: "IX_Workdays_UserId",
                table: "WorkDays",
                newName: "IX_WorkDays_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Workdays_CinemaId",
                table: "WorkDays",
                newName: "IX_WorkDays_CinemaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDays",
                table: "WorkDays",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_WorkDays_WorkDayId",
                table: "Reports",
                column: "WorkDayId",
                principalTable: "WorkDays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDays_AspNetUsers_UserId",
                table: "WorkDays",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDays_Cinemas_CinemaId",
                table: "WorkDays",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id");
        }
    }
}
