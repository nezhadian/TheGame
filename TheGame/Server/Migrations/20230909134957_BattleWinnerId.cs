using Microsoft.EntityFrameworkCore.Migrations;

namespace TheGame.Server.Migrations
{
    public partial class BattleWinnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Users_WinnerId",
                table: "Battles");

            migrationBuilder.DropIndex(
                name: "IX_Battles_WinnerId",
                table: "Battles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Battles_WinnerId",
                table: "Battles",
                column: "WinnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Users_WinnerId",
                table: "Battles",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
