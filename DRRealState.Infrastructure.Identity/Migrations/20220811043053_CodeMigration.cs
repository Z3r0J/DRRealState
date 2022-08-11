using Microsoft.EntityFrameworkCore.Migrations;

namespace DRRealState.Infrastructure.Identity.Migrations
{
    public partial class CodeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Identity",
                table: "Users",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Identity",
                table: "Users");
        }
    }
}
