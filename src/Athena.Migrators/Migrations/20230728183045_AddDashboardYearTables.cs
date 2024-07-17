using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class AddDashboardYearTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "Start",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Catalog",
                table: "TeacherCourseLevelYears");

            migrationBuilder.AddColumn<Guid>(
                name: "DashboardYearId",
                schema: "Catalog",
                table: "Years",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "YearState",
                schema: "Catalog",
                table: "Years",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                schema: "Catalog",
                table: "ExamStudentAnswers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionChoiceId",
                schema: "Catalog",
                table: "ExamStudentAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DashboardYears",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardYears", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Years_DashboardYearId",
                schema: "Catalog",
                table: "Years",
                column: "DashboardYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamStudentAnswers_QuestionChoiceId",
                schema: "Catalog",
                table: "ExamStudentAnswers",
                column: "QuestionChoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamStudentAnswers_QuestionChoices_QuestionChoiceId",
                schema: "Catalog",
                table: "ExamStudentAnswers",
                column: "QuestionChoiceId",
                principalSchema: "Catalog",
                principalTable: "QuestionChoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Years_DashboardYears_DashboardYearId",
                schema: "Catalog",
                table: "Years",
                column: "DashboardYearId",
                principalSchema: "Catalog",
                principalTable: "DashboardYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamStudentAnswers_QuestionChoices_QuestionChoiceId",
                schema: "Catalog",
                table: "ExamStudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Years_DashboardYears_DashboardYearId",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropTable(
                name: "DashboardYears",
                schema: "Catalog");

            migrationBuilder.DropIndex(
                name: "IX_Years_DashboardYearId",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_ExamStudentAnswers_QuestionChoiceId",
                schema: "Catalog",
                table: "ExamStudentAnswers");

            migrationBuilder.DropColumn(
                name: "DashboardYearId",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "YearState",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "QuestionChoiceId",
                schema: "Catalog",
                table: "ExamStudentAnswers");

            migrationBuilder.AddColumn<int>(
                name: "End",
                schema: "Catalog",
                table: "Years",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "Catalog",
                table: "Years",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Start",
                schema: "Catalog",
                table: "Years",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "State",
                schema: "Catalog",
                table: "TeacherCourseLevelYears",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                schema: "Catalog",
                table: "ExamStudentAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
