﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Athena.Migrators.Migrations
{
    public partial class AddPointsForExamGroupStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Points",
                schema: "Catalog",
                table: "ExamGroupStudents",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                schema: "Catalog",
                table: "ExamGroupStudents");
        }
    }
}
