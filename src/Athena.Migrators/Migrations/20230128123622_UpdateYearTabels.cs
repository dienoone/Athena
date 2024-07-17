using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class UpdateYearTabels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "Catalog",
                table: "Years",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "Catalog",
                table: "Years");

            migrationBuilder.AlterColumn<bool>(
                name: "State",
                schema: "Catalog",
                table: "TeacherCourseLevelYearSemsters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
