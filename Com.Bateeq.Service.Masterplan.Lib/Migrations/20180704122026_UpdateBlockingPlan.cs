using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class UpdateBlockingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SMV_Sewing",
                table: "BlockingPlanWorkSchedules",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BlockingPlans",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BlockingPlans");

            migrationBuilder.AlterColumn<int>(
                name: "SMV_Sewing",
                table: "BlockingPlanWorkSchedules",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
