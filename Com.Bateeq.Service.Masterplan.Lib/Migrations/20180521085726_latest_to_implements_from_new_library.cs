using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Migrations
{
    public partial class latest_to_implements_from_new_library : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_CreatedAgent",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_CreatedBy",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_CreatedUtc",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_DeletedAgent",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_DeletedBy",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_DeletedUtc",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_IsDeleted",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_LastModifiedAgent",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_LastModifiedBy",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_LastModifiedUtc",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "_CreatedAgent",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_CreatedBy",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_CreatedUtc",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_DeletedAgent",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_DeletedBy",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_DeletedUtc",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_IsDeleted",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_LastModifiedAgent",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_LastModifiedBy",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "_LastModifiedUtc",
                table: "Commodities");

            migrationBuilder.AddColumn<string>(
                name: "CreatedAgent",
                table: "Sections",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sections",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Sections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedAgent",
                table: "Sections",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Sections",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedUtc",
                table: "Sections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedAgent",
                table: "Sections",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Sections",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedUtc",
                table: "Sections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedAgent",
                table: "Commodities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Commodities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Commodities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedAgent",
                table: "Commodities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Commodities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedUtc",
                table: "Commodities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Commodities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedAgent",
                table: "Commodities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Commodities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedUtc",
                table: "Commodities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAgent",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "DeletedAgent",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "DeletedUtc",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "LastModifiedAgent",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "LastModifiedUtc",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CreatedAgent",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "DeletedAgent",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "DeletedUtc",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "LastModifiedAgent",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "LastModifiedUtc",
                table: "Commodities");

            migrationBuilder.AddColumn<string>(
                name: "_CreatedAgent",
                table: "Sections",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_CreatedBy",
                table: "Sections",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "_CreatedUtc",
                table: "Sections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "_DeletedAgent",
                table: "Sections",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_DeletedBy",
                table: "Sections",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "_DeletedUtc",
                table: "Sections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "_IsDeleted",
                table: "Sections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "_LastModifiedAgent",
                table: "Sections",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_LastModifiedBy",
                table: "Sections",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "_LastModifiedUtc",
                table: "Sections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "_CreatedAgent",
                table: "Commodities",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_CreatedBy",
                table: "Commodities",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "_CreatedUtc",
                table: "Commodities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "_DeletedAgent",
                table: "Commodities",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_DeletedBy",
                table: "Commodities",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "_DeletedUtc",
                table: "Commodities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "_IsDeleted",
                table: "Commodities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "_LastModifiedAgent",
                table: "Commodities",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_LastModifiedBy",
                table: "Commodities",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "_LastModifiedUtc",
                table: "Commodities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
