using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBDcw12.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Pesel", "Telephone" },
                values: new object[,]
                {
                    { 1, "jkowalski@gmail.com", "Jan", "Kowalski", "11111111", "1111111" },
                    { 2, "jnowak@gmail.com", "Julia", "Nowak", "22222222", "222222" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "IdCountry", "Name" },
                values: new object[,]
                {
                    { 1, "Poland" },
                    { 2, "Spain" }
                });

            migrationBuilder.InsertData(
                table: "Trip",
                columns: new[] { "IdTrip", "DateFrom", "DateTo", "Description", "MaxPeople", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "desc", 20, "wyc1" },
                    { 2, new DateTime(2022, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "desc", 30, "wyc2" }
                });

            migrationBuilder.InsertData(
                table: "Country_Trip",
                columns: new[] { "IdCountry", "IdTrip" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Trip_IdTrip",
                table: "Client_Trip",
                column: "IdTrip");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Trip_IdTrip",
                table: "Country_Trip",
                column: "IdTrip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client_Trip");

            migrationBuilder.DropTable(
                name: "Country_Trip");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Trip");
        }
    }
}
