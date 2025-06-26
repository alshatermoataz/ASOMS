using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASOMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderNotesAndPickupTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotesToBuyer",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotesToSeller",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupTime",
                table: "Orders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotesToBuyer",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "NotesToSeller",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PickupTime",
                table: "Orders");
        }
    }
}
