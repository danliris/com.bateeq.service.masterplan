using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class AddCancelRemainingBookingOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialOrderQuantity",
                table: "BookingOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Total",
                table: "BookingOrderDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialOrderQuantity",
                table: "BookingOrders");

            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "BookingOrderDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
