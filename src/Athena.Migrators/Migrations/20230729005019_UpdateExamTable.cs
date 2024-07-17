using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class UpdateExamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_TeacherCourseLevelYearSemsters_TeacherCourseLevelYearSemsterId",
                schema: "Catalog",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "TeacherCourseLevelYearSemsterId",
                schema: "Catalog",
                table: "Exams",
                newName: "TeacherCourseLevelYearId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_TeacherCourseLevelYearSemsterId",
                schema: "Catalog",
                table: "Exams",
                newName: "IX_Exams_TeacherCourseLevelYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_TeacherCourseLevelYears_TeacherCourseLevelYearId",
                schema: "Catalog",
                table: "Exams",
                column: "TeacherCourseLevelYearId",
                principalSchema: "Catalog",
                principalTable: "TeacherCourseLevelYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_TeacherCourseLevelYears_TeacherCourseLevelYearId",
                schema: "Catalog",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "TeacherCourseLevelYearId",
                schema: "Catalog",
                table: "Exams",
                newName: "TeacherCourseLevelYearSemsterId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_TeacherCourseLevelYearId",
                schema: "Catalog",
                table: "Exams",
                newName: "IX_Exams_TeacherCourseLevelYearSemsterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_TeacherCourseLevelYearSemsters_TeacherCourseLevelYearSemsterId",
                schema: "Catalog",
                table: "Exams",
                column: "TeacherCourseLevelYearSemsterId",
                principalSchema: "Catalog",
                principalTable: "TeacherCourseLevelYearSemsters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
