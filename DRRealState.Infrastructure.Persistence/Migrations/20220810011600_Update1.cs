using Microsoft.EntityFrameworkCore.Migrations;

namespace DRRealState.Infrastructure.Persistence.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "DRRealState",
                table: "Estate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "DRRealState",
                table: "Estate");
        }
    }
}
