using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class andriAddFieldisHasRoViewing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RemainingEh",
                table: "BlockingPlanWorkSchedules",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "isHasRoViewing",
                table: "BlockingPlanWorkSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isHasRoViewing",
                table: "BlockingPlanWorkSchedules");

            migrationBuilder.AlterColumn<int>(
                name: "RemainingEh",
                table: "BlockingPlanWorkSchedules",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
