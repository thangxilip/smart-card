using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_SuperMemo_Fields_To_Card : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "current_interval",
                table: "card",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "easiness_factor",
                table: "card",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "next_study_date",
                table: "card",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "started_studying",
                table: "card",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_interval",
                table: "card");

            migrationBuilder.DropColumn(
                name: "easiness_factor",
                table: "card");

            migrationBuilder.DropColumn(
                name: "next_study_date",
                table: "card");

            migrationBuilder.DropColumn(
                name: "started_studying",
                table: "card");
        }
    }
}
