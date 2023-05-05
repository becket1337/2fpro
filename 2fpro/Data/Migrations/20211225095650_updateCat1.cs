using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class updateCat1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomStr",
                table: "Categories",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentCatname",
                table: "Categories",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomStr",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentCatname",
                table: "Categories");
        }
    }
}
