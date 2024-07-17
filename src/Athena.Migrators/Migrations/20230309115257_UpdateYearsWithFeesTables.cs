using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class UpdateYearsWithFeesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntroFee",
                schema: "Catalog",
                table: "TeacherCourseLevelYearStudents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntroFee",
                schema: "Catalog",
                table: "TeacherCourseLevelYears",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonthFee",
                schema: "Catalog",
                table: "TeacherCourseLevelYears",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntroFee",
                schema: "Catalog",
                table: "TeacherCourseLevelYearStudents");

            migrationBuilder.DropColumn(
                name: "IntroFee",
                schema: "Catalog",
                table: "TeacherCourseLevelYears");

            migrationBuilder.DropColumn(
                name: "MonthFee",
                schema: "Catalog",
                table: "TeacherCourseLevelYears");
        }
    }
}
