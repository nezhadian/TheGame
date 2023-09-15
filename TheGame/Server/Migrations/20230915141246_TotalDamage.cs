using Microsoft.EntityFrameworkCore.Migrations;

namespace TheGame.Server.Migrations
{
    public partial class TotalDamage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalDamage",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDamage",
                table: "Users");
        }
    }
}
