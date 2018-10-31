using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class updatefieldforcanceleditem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canceledItem",
                table: "BookingOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "canceledItem",
                table: "BookingOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canceledItem",
                table: "BookingOrders");

            migrationBuilder.AddColumn<int>(
                name: "canceledItem",
                table: "BookingOrderDetails",
                nullable: false,
                defaultValue: 0);
        }
    }
}
