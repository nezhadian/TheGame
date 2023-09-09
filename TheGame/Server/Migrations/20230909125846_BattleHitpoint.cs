using Microsoft.EntityFrameworkCore.Migrations;

namespace TheGame.Server.Migrations
{
    public partial class BattleHitpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttackerHitpoint",
                table: "Battles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpponentHitpoint",
                table: "Battles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttackerHitpoint",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "OpponentHitpoint",
                table: "Battles");
        }
    }
}
