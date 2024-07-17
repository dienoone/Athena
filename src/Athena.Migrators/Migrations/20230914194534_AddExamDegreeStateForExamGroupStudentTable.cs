using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class AddExamDegreeStateForExamGroupStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamDegreeState",
                schema: "Catalog",
                table: "ExamGroupStudents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Absent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamDegreeState",
                schema: "Catalog",
                table: "ExamGroupStudents");
        }
    }
}
