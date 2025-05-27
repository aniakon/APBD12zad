using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBDcw12.Migrations
{
    /// <inheritdoc />
    public partial class SeedClientTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client_Trip",
                columns: new[] { "IdClient", "IdTrip", "PaymentDate", "RegisteredAt" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2022, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, null, new DateTime(2022, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, null, new DateTime(2022, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Client_Trip",
                keyColumns: new[] { "IdClient", "IdTrip" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Client_Trip",
                keyColumns: new[] { "IdClient", "IdTrip" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Client_Trip",
                keyColumns: new[] { "IdClient", "IdTrip" },
                keyValues: new object[] { 2, 2 });
        }
    }
}
