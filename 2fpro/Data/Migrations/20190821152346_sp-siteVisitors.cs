using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class spsiteVisitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var spSiteVisitors = @"
             CREATE PROCEDURE dbo.GetUsersOnline
				  
				  @Days int=0, @IspName varchar(30) = '%test%'
				  as
                    BEGIN
                        declare @facebookBot varchar(50)= '%Facebook%';
                        declare @AmazonBot varchar(50)= '%Amazon%';
                        SET NOCOUNT ON;
                            select * from Diagnostics
                            Where DateDiff(day, CurrDateTime, Getdate()) <= @Days
                            AND DateDiff(minute,CurrDateTime,Getdate())>2
                            AND ConnectionID<> ''
                            AND ISPName not like @facebookBot
                            AND ISPName not like @AmazonBot
                            AND ISPName not like @IspName
                            AND ISPName is not null
                    END
					go";


            migrationBuilder.Sql(spSiteVisitors);


            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2019, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP PROCEDURE dbo.GetUsersOnline");

            migrationBuilder.UpdateData(
                table: "Menues",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedDate",
                value: new DateTime(2019, 2, 16, 17, 32, 53, 807, DateTimeKind.Local).AddTicks(5960));
        }
    }
}
