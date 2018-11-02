using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class andriAddFieldBookingOrderDetailIsConfirmDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAddNew",
                table: "BookingOrderDetails",
                newName: "IsAddNew");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmDelete",
                table: "BookingOrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmDelete",
                table: "BookingOrderDetails");

            migrationBuilder.RenameColumn(
                name: "IsAddNew",
                table: "BookingOrderDetails",
                newName: "isAddNew");
        }
    }
}
