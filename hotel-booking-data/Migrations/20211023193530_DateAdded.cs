using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hotel_booking_data.Migrations
{
    public partial class DateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmationFlag",
                table: "ManagerRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "ManagerRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationFlag",
                table: "ManagerRequests");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "ManagerRequests");
        }
    }
}
