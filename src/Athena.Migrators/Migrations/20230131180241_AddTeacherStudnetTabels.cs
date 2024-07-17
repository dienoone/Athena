using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class AddTeacherStudnetTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherCourseLevelHeadQuarters",
                schema: "Catalog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherCourseLevelHeadQuarters",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeadQuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TeacherCourseLevelHeadQuarters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherCourseLevelHeadQuarters_HeadQuarters_HeadQuarterId",
                        column: x => x.HeadQuarterId,
                        principalSchema: "Catalog",
                        principalTable: "HeadQuarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherCourseLevelHeadQuarters_TeacherCourseLevels_TeacherCourseLevelId",
                        column: x => x.TeacherCourseLevelId,
                        principalSchema: "Catalog",
                        principalTable: "TeacherCourseLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseLevelHeadQuarters_HeadQuarterId",
                schema: "Catalog",
                table: "TeacherCourseLevelHeadQuarters",
                column: "HeadQuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourseLevelHeadQuarters_TeacherCourseLevelId",
                schema: "Catalog",
                table: "TeacherCourseLevelHeadQuarters",
                column: "TeacherCourseLevelId");
        }
    }
}
