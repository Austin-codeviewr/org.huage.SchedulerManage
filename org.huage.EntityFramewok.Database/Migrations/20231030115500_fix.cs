using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace org.huage.EntityFramewok.Database.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Scheduler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Scheduler",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JobStatus",
                table: "Scheduler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MethodParams",
                table: "Scheduler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Scheduler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "Scheduler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "StartTime",
                table: "Scheduler",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Scheduler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Scheduler",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Scheduler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "JobStatus",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "MethodParams",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Scheduler");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Scheduler");
        }
    }
}
