using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DepoYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class newSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductModel_ProductModelId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductModel");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStocks",
                table: "ProductStocks");

            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_ProductId",
                table: "ProductStocks");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductModelId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductModelId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Interface",
                table: "Storages",
                newName: "Model");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Storages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Rams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Rams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStocks",
                table: "ProductStocks",
                columns: new[] { "ProductId", "WarehouseId" });

            migrationBuilder.InsertData(
                table: "Cpus",
                columns: new[] { "CpuId", "BaseClockGHz", "BoostClockGHz", "Cores", "Manufacturer", "Model", "Threads" },
                values: new object[,]
                {
                    { 1, 2.5, 4.4000000000000004, 6, "Intel", "Core i5-12400F", 12 },
                    { 2, 3.7000000000000002, 4.5999999999999996, 6, "AMD", "Ryzen 5 5600X", 12 }
                });

            migrationBuilder.InsertData(
                table: "Gpus",
                columns: new[] { "GpuId", "Manufacturer", "Memory", "Model" },
                values: new object[,]
                {
                    { 1, "NVIDIA", "12GB GDDR6", "RTX 3060" },
                    { 2, "AMD", "8GB GDDR6", "RX 6600 XT" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Aisle", "Bin", "Shelf" },
                values: new object[,]
                {
                    { 1, "A", "01", "1" },
                    { 2, "A", "05", "2" },
                    { 3, "B", "02", "1" },
                    { 4, "C", "04", "3" }
                });

            migrationBuilder.InsertData(
                table: "Rams",
                columns: new[] { "RamId", "Manufacturer", "Model", "SizeGB", "SpeedMHz", "Type" },
                values: new object[,]
                {
                    { 1, "Corsair", "Vengeance LPX", 16, 3200, "DDR4" },
                    { 2, "Kingston", "Fury Beast", 32, 3600, "DDR4" }
                });

            migrationBuilder.InsertData(
                table: "Screens",
                columns: new[] { "ScreenId", "PanelType", "RefreshRate", "Resolution", "SizeInches" },
                values: new object[,]
                {
                    { 1, "IPS", 60, "3840x2160", 27.0 },
                    { 2, "IPS", 165, "1920x1080", 24.0 }
                });

            migrationBuilder.InsertData(
                table: "Storages",
                columns: new[] { "StorageId", "CapacityGB", "Manufacturer", "Model", "Type" },
                values: new object[,]
                {
                    { 1, 1000, "Samsung", "970 EVO Plus", "NVMe SSD" },
                    { 2, 1000, "Seagate", "Barracuda", "HDD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStocks",
                table: "ProductStocks");

            migrationBuilder.DeleteData(
                table: "Cpus",
                keyColumn: "CpuId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cpus",
                keyColumn: "CpuId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Gpus",
                keyColumn: "GpuId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Gpus",
                keyColumn: "GpuId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rams",
                keyColumn: "RamId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rams",
                keyColumn: "RamId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Screens",
                keyColumn: "ScreenId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Screens",
                keyColumn: "ScreenId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Storages",
                keyColumn: "StorageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Storages",
                keyColumn: "StorageId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Storages",
                newName: "Interface");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductStocks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductStocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductModelId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStocks",
                table: "ProductStocks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "ProductModel",
                columns: table => new
                {
                    ProductModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModel", x => x.ProductModelId);
                    table.ForeignKey(
                        name: "FK_ProductModel_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_ProductId",
                table: "ProductStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductModelId",
                table: "Products",
                column: "ProductModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModel_BrandId",
                table: "ProductModel",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductModel_ProductModelId",
                table: "Products",
                column: "ProductModelId",
                principalTable: "ProductModel",
                principalColumn: "ProductModelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
