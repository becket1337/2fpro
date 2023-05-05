using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class InitDiagnostics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UsersOnlineStatus = table.Column<int>(nullable: false),
                    AuthUsersOnlineStatus = table.Column<int>(nullable: false),
                    TheMostPopularLocation = table.Column<string>(nullable: true),
                    IsActivate = table.Column<bool>(nullable: false),
                    CustomStringField = table.Column<string>(nullable: true),
                    CustomStringField2 = table.Column<string>(nullable: true),
                    CustomIntField3 = table.Column<int>(nullable: false),
                    CurrDateTime = table.Column<DateTime>(nullable: false),
                    CustomBoolField = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.ID);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2018, 12, 25, 22, 10, 1, 974, DateTimeKind.Local));
        }
    }
}
