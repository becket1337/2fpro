using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class InitGeolocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Diagnostics",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "TheMostPopularLocation",
                table: "Diagnostics",
                newName: "Zip");

            migrationBuilder.RenameColumn(
                name: "CustomStringField2",
                table: "Diagnostics",
                newName: "Timezone");

            migrationBuilder.RenameColumn(
                name: "CustomIntField3",
                table: "Diagnostics",
                newName: "CustomIntField");

            migrationBuilder.AddColumn<string>(
                name: "AsNumberAndName",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionID",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISPName",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMobileConn",
                table: "Diagnostics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Diagnostics",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Diagnostics",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionCode",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Diagnostics",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2019, 1, 22, 23, 25, 43, 762, DateTimeKind.Local).AddTicks(4808));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsNumberAndName",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "ConnectionID",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "ISPName",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "IsMobileConn",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Diagnostics");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Diagnostics");

            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "Diagnostics",
                newName: "TheMostPopularLocation");

            migrationBuilder.RenameColumn(
                name: "Timezone",
                table: "Diagnostics",
                newName: "CustomStringField2");

            migrationBuilder.RenameColumn(
                name: "CustomIntField",
                table: "Diagnostics",
                newName: "CustomIntField3");

            migrationBuilder.InsertData(
                table: "Diagnostics",
                columns: new[] { "ID", "AuthUsersOnlineStatus", "CurrDateTime", "CustomBoolField", "CustomIntField3", "CustomStringField", "CustomStringField2", "IsActivate", "TheMostPopularLocation", "UsersOnlineStatus" },
                values: new object[] { 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 0, null, null, false, null, 0 });

            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2019, 1, 21, 21, 12, 4, 178, DateTimeKind.Local).AddTicks(7099));
        }
    }
}
