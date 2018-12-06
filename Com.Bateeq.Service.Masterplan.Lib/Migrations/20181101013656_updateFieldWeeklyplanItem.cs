using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class updateFieldWeeklyplanItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "UsedEh",
                table: "WeeklyPlanItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "RemainingEh",
                table: "WeeklyPlanItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "EhTotal",
                table: "WeeklyPlanItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "AhTotal",
                table: "WeeklyPlanItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UsedEh",
                table: "WeeklyPlanItems",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "RemainingEh",
                table: "WeeklyPlanItems",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "EhTotal",
                table: "WeeklyPlanItems",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "AhTotal",
                table: "WeeklyPlanItems",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
