using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class RemoveBookingOrderFKOnBlockingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockingPlans_BookingOrders_BookingOrderId",
                table: "BlockingPlans");

            migrationBuilder.DropIndex(
                name: "IX_BlockingPlans_BookingOrderId",
                table: "BlockingPlans");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrders_BlockingPlanId",
                table: "BookingOrders",
                column: "BlockingPlanId",
                unique: true,
                filter: "[BlockingPlanId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOrders_BlockingPlans_BlockingPlanId",
                table: "BookingOrders",
                column: "BlockingPlanId",
                principalTable: "BlockingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOrders_BlockingPlans_BlockingPlanId",
                table: "BookingOrders");

            migrationBuilder.DropIndex(
                name: "IX_BookingOrders_BlockingPlanId",
                table: "BookingOrders");

            migrationBuilder.CreateIndex(
                name: "IX_BlockingPlans_BookingOrderId",
                table: "BlockingPlans",
                column: "BookingOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockingPlans_BookingOrders_BookingOrderId",
                table: "BlockingPlans",
                column: "BookingOrderId",
                principalTable: "BookingOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
