using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class UpdateTeacherTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                schema: "Catalog",
                table: "Teachers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDay",
                schema: "Catalog",
                table: "Teachers");
        }
    }
}
