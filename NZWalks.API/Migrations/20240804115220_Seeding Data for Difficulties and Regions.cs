using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataforDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("250713a7-58ab-43a9-8256-0574718edde3"), "Easy" },
                    { new Guid("51d91533-e380-40bd-b0b9-2ab226885ef2"), "Medium" },
                    { new Guid("bbbc3995-0041-440a-bc04-a327c9b4fc89"), "Hard" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("f1b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"), "NO", "https://www.doc.govt.nz/globalassets/images/places/northland/northland-landscape-1.jpg", "Northland" },
                    { new Guid("f2b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"), "AU", "https://www.doc.govt.nz/globalassets/images/places/auckland/auckland-landscape-1.jpg", "Auckland" },
                    { new Guid("f3b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"), "WK", "https://www.doc.govt.nz/globalassets/images/places/waikato/waikato-landscape-1.jpg", "Waikato" },
                    { new Guid("f4b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"), "BP", "https://www.doc.govt.nz/globalassets/images/places/bay-of-plenty/bay-of-plenty-landscape-1.jpg", "Bay of Plenty" },
                    { new Guid("f5b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"), "GI", "https://www.doc.govt.nz/globalassets/images/places/gisborne/gisborne-landscape-1.jpg", "Gisborne" },
                    { new Guid("f6b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"), "HB", "https://www.doc.govt.nz/globalassets/images/places/hawkes-bay/hawkes-bay-landscape-1.jpg", "Hawke's Bay" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("250713a7-58ab-43a9-8256-0574718edde3"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("51d91533-e380-40bd-b0b9-2ab226885ef2"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("bbbc3995-0041-440a-bc04-a327c9b4fc89"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f1b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f2b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f3b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f4b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f5b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f6b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"));
        }
    }
}
