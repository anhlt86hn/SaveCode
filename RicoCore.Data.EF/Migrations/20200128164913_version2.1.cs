using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RicoCore.Data.EF.Migrations
{
    public partial class version21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "SubProjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "SubProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "SubProjects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Folders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Folders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Folders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Files",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "SubProjects");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "SubProjects");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "SubProjects");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Files");
        }
    }
}
