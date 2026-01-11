using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecurityIncidentTrackerWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedTechnicianDepartmens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechnicianDepartments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicianID = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianDepartments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TechnicianDepartments_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicianDepartments_Technicians_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "Technicians",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianDepartments_DepartmentID",
                table: "TechnicianDepartments",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianDepartments_TechnicianID",
                table: "TechnicianDepartments",
                column: "TechnicianID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicianDepartments");
        }
    }
}
