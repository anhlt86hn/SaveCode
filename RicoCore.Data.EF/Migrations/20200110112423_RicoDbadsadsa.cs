using Microsoft.EntityFrameworkCore.Migrations;

namespace RicoCore.Data.EF.Migrations
{
    public partial class RicoDbadsadsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Functions",
                nullable: false,
                oldClrType: typeof(bool));

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsActive",
            //    table: "Functions",
            //    nullable: false,
            //    defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "IsActive",
            //    table: "Functions");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Functions",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
