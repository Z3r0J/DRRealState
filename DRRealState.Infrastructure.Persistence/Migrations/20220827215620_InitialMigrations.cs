using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DRRealState.Infrastructure.Persistence.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DRRealState");

            migrationBuilder.CreateTable(
                name: "PropertyType",
                schema: "DRRealState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleType",
                schema: "DRRealState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Upgrade",
                schema: "DRRealState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upgrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estate",
                schema: "DRRealState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BathroomQuantity = table.Column<int>(type: "int", nullable: false),
                    BedRoomQuantity = table.Column<int>(type: "int", nullable: false),
                    SizeInMeters = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubication = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    SaleTypeId = table.Column<int>(type: "int", nullable: false),
                    AgentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estate_PropertyType_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalSchema: "DRRealState",
                        principalTable: "PropertyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estate_SaleType_SaleTypeId",
                        column: x => x.SaleTypeId,
                        principalSchema: "DRRealState",
                        principalTable: "SaleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estate_Favorite",
                schema: "DRRealState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstateId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estate_Favorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estate_Favorite_Estate_EstateId",
                        column: x => x.EstateId,
                        principalSchema: "DRRealState",
                        principalTable: "Estate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                schema: "DRRealState",
                columns: table => new
                {
                    GalleryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.GalleryId);
                    table.ForeignKey(
                        name: "FK_Gallery_Estate_EstateId",
                        column: x => x.EstateId,
                        principalSchema: "DRRealState",
                        principalTable: "Estate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Upgrade_Estate",
                schema: "DRRealState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstateId = table.Column<int>(type: "int", nullable: false),
                    UpgradeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upgrade_Estate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Upgrade_Estate_Estate_EstateId",
                        column: x => x.EstateId,
                        principalSchema: "DRRealState",
                        principalTable: "Estate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Upgrade_Estate_Upgrade_UpgradeId",
                        column: x => x.UpgradeId,
                        principalSchema: "DRRealState",
                        principalTable: "Upgrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estate_PropertyTypeId",
                schema: "DRRealState",
                table: "Estate",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Estate_SaleTypeId",
                schema: "DRRealState",
                table: "Estate",
                column: "SaleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Estate_Favorite_EstateId",
                schema: "DRRealState",
                table: "Estate_Favorite",
                column: "EstateId");

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_EstateId",
                schema: "DRRealState",
                table: "Gallery",
                column: "EstateId");

            migrationBuilder.CreateIndex(
                name: "IX_Upgrade_Estate_EstateId",
                schema: "DRRealState",
                table: "Upgrade_Estate",
                column: "EstateId");

            migrationBuilder.CreateIndex(
                name: "IX_Upgrade_Estate_UpgradeId",
                schema: "DRRealState",
                table: "Upgrade_Estate",
                column: "UpgradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estate_Favorite",
                schema: "DRRealState");

            migrationBuilder.DropTable(
                name: "Gallery",
                schema: "DRRealState");

            migrationBuilder.DropTable(
                name: "Upgrade_Estate",
                schema: "DRRealState");

            migrationBuilder.DropTable(
                name: "Estate",
                schema: "DRRealState");

            migrationBuilder.DropTable(
                name: "Upgrade",
                schema: "DRRealState");

            migrationBuilder.DropTable(
                name: "PropertyType",
                schema: "DRRealState");

            migrationBuilder.DropTable(
                name: "SaleType",
                schema: "DRRealState");
        }
    }
}
