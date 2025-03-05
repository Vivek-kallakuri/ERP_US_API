using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortracker360_Accurate_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");
        }
    }
}
