using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class updateSortindexes_Menu_StaticSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sequance",
                table: "StaticSections",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR GetSeq",
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2018, 12, 25, 22, 10, 1, 974, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 1,
                column: "Sequance",
                value: 0);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 2,
                column: "Sequance",
                value: 0);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 3,
                column: "Sequance",
                value: 0);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 4,
                column: "Sequance",
                value: 0);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 5,
                column: "Sequance",
                value: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sequance",
                table: "StaticSections",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValueSql: "NEXT VALUE FOR GetSeq");

            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2018, 12, 25, 21, 7, 50, 94, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 1,
                column: "Sequance",
                value: 1);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 2,
                column: "Sequance",
                value: 2);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 3,
                column: "Sequance",
                value: 3);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 4,
                column: "Sequance",
                value: 4);

            migrationBuilder.UpdateData(
                table: "StaticSections",
                keyColumn: "ID",
                keyValue: 5,
                column: "Sequance",
                value: 5);
        }
    }
}
