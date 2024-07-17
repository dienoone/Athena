using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class addGroupAndStudentTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadQuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCourseLevelYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_HeadQuarters_HeadQuarterId",
                        column: x => x.HeadQuarterId,
                        principalSchema: "Catalog",
                        principalTable: "HeadQuarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_TeacherCourseLevelYears_TeacherCourseLevelYearId",
                        column: x => x.TeacherCourseLevelYearId,
                        principalSchema: "Catalog",
                        principalTable: "TeacherCourseLevelYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherCourseLevelStudents",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCourseLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupScaduals",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_GroupScaduals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupScaduals_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Catalog",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupStudents",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCourseLevelStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_GroupStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupStudents_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Catalog",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupStudents_TeacherCourseLevelStudents_TeacherCourseLevelStudentId",
                        column: x => x.TeacherCourseLevelStudentId,
                        principalSchema: "Catalog",
                        principalTable: "TeacherCourseLevelStudents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_HeadQuarterId",
                schema: "Catalog",
                table: "Groups",
                column: "HeadQuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherCourseLevelYearId",
                schema: "Catalog",
                table: "Groups",
                column: "TeacherCourseLevelYearId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupScaduals_GroupId",
                schema: "Catalog",
                table: "GroupScaduals",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_GroupId",
                schema: "Catalog",
                table: "GroupStudents",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_TeacherCourseLevelStudentId",
                schema: "Catalog",
                table: "GroupStudents",
                column: "TeacherCourseLevelStudentId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupScaduals",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "GroupStudents",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "TeacherCourseLevelStudents",
                schema: "Catalog");
        }
    }
}
