using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class RetroalimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Area", "CreatedAt", "Description", "ImageUrl", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 60, new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(995), "Description test", "https://image.com", "villa test", 999.0, new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(1011) },
                    { 2, 40, new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(1013), "Description test 02", "https://image02.com", "villa test 02", 990.0, new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(1014) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
