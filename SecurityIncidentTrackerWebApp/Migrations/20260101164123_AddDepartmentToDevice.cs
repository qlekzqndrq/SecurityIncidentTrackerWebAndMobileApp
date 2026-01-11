using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecurityIncidentTrackerWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Devices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DepartmentID",
                table: "Devices",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Departments_DepartmentID",
                table: "Devices",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Departments_DepartmentID",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DepartmentID",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Devices");
        }
    }
}
