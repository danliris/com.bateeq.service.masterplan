using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class latestOn_04_47PM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Commodities_Code",
                table: "Commodities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Commodities_Code",
                table: "Commodities",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }
    }
}
