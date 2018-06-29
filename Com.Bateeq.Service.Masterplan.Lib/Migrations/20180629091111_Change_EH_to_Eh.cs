using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class Change_EH_to_Eh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingEH",
                table: "BlockingPlanWorkSchedules",
                newName: "RemainingEh");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingEh",
                table: "BlockingPlanWorkSchedules",
                newName: "RemainingEH");
        }
    }
}
