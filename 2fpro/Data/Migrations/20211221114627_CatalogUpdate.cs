using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class CatalogUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<bool>(
                name: "ToYaMarket",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DoShow",
                table: "Categories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageDataType",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "YaMakrets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    UrlSite = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    SiteName = table.Column<string>(nullable: true),
                    Delivery = table.Column<string>(nullable: true),
                    Pickup = table.Column<string>(nullable: true),
                    SalesNotes = table.Column<string>(maxLength: 50, nullable: true),
                    Condition = table.Column<string>(nullable: true),
                    Gifts = table.Column<string>(nullable: true),
                    Promos = table.Column<string>(nullable: true),
                    SomeValue = table.Column<int>(nullable: false),
                    SomeValueTwo = table.Column<int>(nullable: false),
                    SomeStrValue = table.Column<string>(nullable: true),
                    SomeStrValueTwo = table.Column<string>(nullable: true),
                    SomeDecValue = table.Column<decimal>(nullable: false),
                    SomeBoolValue = table.Column<bool>(nullable: false),
                    SomeBoolValueTwo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YaMakrets", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "YaMakrets",
                columns: new[] { "ID", "CompanyName", "Condition", "Currency", "Delivery", "Email", "Gifts", "PhoneNumber", "Pickup", "Promos", "SalesNotes", "SiteName", "SomeBoolValue", "SomeBoolValueTwo", "SomeDecValue", "SomeStrValue", "SomeStrValueTwo", "SomeValue", "SomeValueTwo", "UrlSite" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, null, null, false, false, 0m, null, null, 0, 0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YaMakrets");

            migrationBuilder.DropColumn(
                name: "ToYaMarket",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DoShow",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageDataType",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Categories");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
