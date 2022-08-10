using Microsoft.EntityFrameworkCore.Migrations;

namespace DRRealState.Infrastructure.Persistence.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estate_PropertyType_PropertiesTypeId",
                schema: "DRRealState",
                table: "Estate");

            migrationBuilder.DropIndex(
                name: "IX_Estate_PropertiesTypeId",
                schema: "DRRealState",
                table: "Estate");

            migrationBuilder.DropColumn(
                name: "PropertiesTypeId",
                schema: "DRRealState",
                table: "Estate");

            migrationBuilder.CreateIndex(
                name: "IX_Estate_PropertyTypeId",
                schema: "DRRealState",
                table: "Estate",
                column: "PropertyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estate_PropertyType_PropertyTypeId",
                schema: "DRRealState",
                table: "Estate",
                column: "PropertyTypeId",
                principalSchema: "DRRealState",
                principalTable: "PropertyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estate_PropertyType_PropertyTypeId",
                schema: "DRRealState",
                table: "Estate");

            migrationBuilder.DropIndex(
                name: "IX_Estate_PropertyTypeId",
                schema: "DRRealState",
                table: "Estate");

            migrationBuilder.AddColumn<int>(
                name: "PropertiesTypeId",
                schema: "DRRealState",
                table: "Estate",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estate_PropertiesTypeId",
                schema: "DRRealState",
                table: "Estate",
                column: "PropertiesTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estate_PropertyType_PropertiesTypeId",
                schema: "DRRealState",
                table: "Estate",
                column: "PropertiesTypeId",
                principalSchema: "DRRealState",
                principalTable: "PropertyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
