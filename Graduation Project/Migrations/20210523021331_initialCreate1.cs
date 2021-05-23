using Microsoft.EntityFrameworkCore.Migrations;

namespace Graduation_Project.Migrations
{
    public partial class initialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDateYear",
                table: "AspNetUsers",
                newName: "BirthDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "AspNetUsers",
                newName: "BirthDateYear");
        }
    }
}
