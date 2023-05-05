using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class PostImgTypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomStr",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SecondCustomStr",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visitors",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomStr",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SecondCustomStr",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Visitors",
                table: "Posts");
        }
    }
}
