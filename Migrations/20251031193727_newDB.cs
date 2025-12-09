using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepoYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class newDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "ProductModelId");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "SerialNumber");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Products",
                type: "int",
                nullable: true);

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
                name: "Cpus",
                columns: table => new
                {
                    CpuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cores = table.Column<int>(type: "int", nullable: false),
                    Threads = table.Column<int>(type: "int", nullable: false),
                    BaseClockGHz = table.Column<double>(type: "float", nullable: false),
                    BoostClockGHz = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.CpuId);
                });

            migrationBuilder.CreateTable(
                name: "Gpus",
                columns: table => new
                {
                    GpuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Memory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gpus", x => x.GpuId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aisle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shelf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Rams",
                columns: table => new
                {
                    RamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeGB = table.Column<int>(type: "int", nullable: false),
                    SpeedMHz = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rams", x => x.RamId);
                });

            migrationBuilder.CreateTable(
                name: "Screens",
                columns: table => new
                {
                    ScreenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeInches = table.Column<double>(type: "float", nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshRate = table.Column<int>(type: "int", nullable: true),
                    PanelType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screens", x => x.ScreenId);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    StorageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapacityGB = table.Column<int>(type: "int", nullable: false),
                    Interface = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.StorageId);
                });

            migrationBuilder.CreateTable(
                name: "ProductModel",
                columns: table => new
                {
                    ProductModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ProductCpus",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CpuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCpus", x => new { x.ProductId, x.CpuId });
                    table.ForeignKey(
                        name: "FK_ProductCpus_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "CpuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCpus_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductGpus",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    GpuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGpus", x => new { x.ProductId, x.GpuId });
                    table.ForeignKey(
                        name: "FK_ProductGpus_Gpus_GpuId",
                        column: x => x.GpuId,
                        principalTable: "Gpus",
                        principalColumn: "GpuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductGpus_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRams",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRams", x => new { x.ProductId, x.RamId });
                    table.ForeignKey(
                        name: "FK_ProductRams_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRams_Rams_RamId",
                        column: x => x.RamId,
                        principalTable: "Rams",
                        principalColumn: "RamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductScreens",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ScreenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductScreens", x => new { x.ProductId, x.ScreenId });
                    table.ForeignKey(
                        name: "FK_ProductScreens_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductScreens_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screens",
                        principalColumn: "ScreenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStorages",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    StorageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStorages", x => new { x.ProductId, x.StorageId });
                    table.ForeignKey(
                        name: "FK_ProductStorages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStorages_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "StorageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_LocationId",
                table: "Products",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductModelId",
                table: "Products",
                column: "ProductModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCpus_CpuId",
                table: "ProductCpus",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGpus_GpuId",
                table: "ProductGpus",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModel_BrandId",
                table: "ProductModel",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRams_RamId",
                table: "ProductRams",
                column: "RamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductScreens_ScreenId",
                table: "ProductScreens",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStorages_StorageId",
                table: "ProductStorages",
                column: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Locations_LocationId",
                table: "Products",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductModel_ProductModelId",
                table: "Products",
                column: "ProductModelId",
                principalTable: "ProductModel",
                principalColumn: "ProductModelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Locations_LocationId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductModel_ProductModelId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "ProductCpus");

            migrationBuilder.DropTable(
                name: "ProductGpus");

            migrationBuilder.DropTable(
                name: "ProductModel");

            migrationBuilder.DropTable(
                name: "ProductRams");

            migrationBuilder.DropTable(
                name: "ProductScreens");

            migrationBuilder.DropTable(
                name: "ProductStorages");

            migrationBuilder.DropTable(
                name: "Cpus");

            migrationBuilder.DropTable(
                name: "Gpus");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Rams");

            migrationBuilder.DropTable(
                name: "Screens");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_LocationId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductModelId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Products",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "ProductModelId",
                table: "Products",
                newName: "Stock");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
