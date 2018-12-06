using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class lucky_addFieldForCancelAndDeleteBookingOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CanceledBookingOrder",
                table: "BookingOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CanceledDate",
                table: "BookingOrders",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "ExpiredBookingOrder",
                table: "BookingOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredDeletedDate",
                table: "BookingOrders",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledBookingOrder",
                table: "BookingOrders");

            migrationBuilder.DropColumn(
                name: "CanceledDate",
                table: "BookingOrders");

            migrationBuilder.DropColumn(
                name: "ExpiredBookingOrder",
                table: "BookingOrders");

            migrationBuilder.DropColumn(
                name: "ExpiredDeletedDate",
                table: "BookingOrders");
        }
    }
}
