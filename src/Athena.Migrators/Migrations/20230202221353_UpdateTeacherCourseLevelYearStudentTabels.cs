using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class UpdateTeacherCourseLevelYearStudentTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_TeacherCourseLevelStudents_TeacherCourseLevelStudentId",
                schema: "Catalog",
                table: "GroupStudents");

            migrationBuilder.DropTable(
                name: "TeacherCourseLevelStudents",
                schema: "Catalog");

            migrationBuilder.RenameColumn(
                name: "TeacherCourseLevelStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                newName: "TeacherCourseLevelYearStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupStudents_TeacherCourseLevelStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                newName: "IX_GroupStudents_TeacherCourseLevelYearStudentId");

            migrationBuilder.CreateTable(
                name: "TeacherCourseLevelYearStudents",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCourseLevelYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourseLevelYearStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherCourseLevelYearStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Catalog",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherCourseLevelYearStudents_TeacherCourseLevelYears_TeacherCourseLevelYearId",
                        column: x => x.TeacherCourseLevelYearId,
                        principalSchema: "Catalog",
                        principalTable: "TeacherCourseLevelYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseLevelYearStudents_StudentId",
                schema: "Catalog",
                table: "TeacherCourseLevelYearStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseLevelYearStudents_TeacherCourseLevelYearId",
                schema: "Catalog",
                table: "TeacherCourseLevelYearStudents",
                column: "TeacherCourseLevelYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_TeacherCourseLevelYearStudents_TeacherCourseLevelYearStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                column: "TeacherCourseLevelYearStudentId",
                principalSchema: "Catalog",
                principalTable: "TeacherCourseLevelYearStudents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_TeacherCourseLevelYearStudents_TeacherCourseLevelYearStudentId",
                schema: "Catalog",
                table: "GroupStudents");

            migrationBuilder.DropTable(
                name: "TeacherCourseLevelYearStudents",
                schema: "Catalog");

            migrationBuilder.RenameColumn(
                name: "TeacherCourseLevelYearStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                newName: "TeacherCourseLevelStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupStudents_TeacherCourseLevelYearStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                newName: "IX_GroupStudents_TeacherCourseLevelStudentId");

            migrationBuilder.CreateTable(
                name: "TeacherCourseLevelStudents",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCourseLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourseLevelStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherCourseLevelStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Catalog",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherCourseLevelStudents_TeacherCourseLevels_TeacherCourseLevelId",
                        column: x => x.TeacherCourseLevelId,
                        principalSchema: "Catalog",
                        principalTable: "TeacherCourseLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseLevelStudents_StudentId",
                schema: "Catalog",
                table: "TeacherCourseLevelStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseLevelStudents_TeacherCourseLevelId",
                schema: "Catalog",
                table: "TeacherCourseLevelStudents",
                column: "TeacherCourseLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_TeacherCourseLevelStudents_TeacherCourseLevelStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                column: "TeacherCourseLevelStudentId",
                principalSchema: "Catalog",
                principalTable: "TeacherCourseLevelStudents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
