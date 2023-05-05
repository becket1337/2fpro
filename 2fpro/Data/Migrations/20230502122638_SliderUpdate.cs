using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class SliderUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProdLink",
                table: "Portfolios",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "Portfolios",
                maxLength: 200,
                nullable: true);

            migrationBuilder.InsertData(
                table: "StaticSections",
                columns: new[] { "ID", "Content", "CreatedAt", "Preview", "SectionType", "Title", "Type" },
                values: new object[] { 10, "ss", null, null, 10, "static10", 0 });

            migrationBuilder.InsertData(
                table: "StaticSections",
                columns: new[] { "ID", "Content", "CreatedAt", "Preview", "SectionType", "Title", "Type" },
                values: new object[] { 11, "ss", null, null, 11, "static11", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "Portfolios");

            migrationBuilder.AlterColumn<string>(
                name: "ProdLink",
                table: "Portfolios",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
