using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class HeadQuarterTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeadQuarters",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Building = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_HeadQuarters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeadQuarters_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "Catalog",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeadQuarterPhones",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadQuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_HeadQuarterPhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeadQuarterPhones_HeadQuarters_HeadQuarterId",
                        column: x => x.HeadQuarterId,
                        principalSchema: "Catalog",
                        principalTable: "HeadQuarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherCourseLevelHeadQuarters",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherCourseLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeadQuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeadQuarterPhones_HeadQuarterId",
                schema: "Catalog",
                table: "HeadQuarterPhones",
                column: "HeadQuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadQuarters_TeacherId",
                schema: "Catalog",
                table: "HeadQuarters",
                column: "TeacherId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeadQuarterPhones",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "TeacherCourseLevelHeadQuarters",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "HeadQuarters",
                schema: "Catalog");
        }
    }
}
