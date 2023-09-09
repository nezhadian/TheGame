using Microsoft.EntityFrameworkCore.Migrations;

namespace TheGame.Server.Migrations
{
    public partial class AddSomePropsToAttackEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDamageForAttacker",
                table: "Attacks",
                newName: "Round");

            migrationBuilder.AddColumn<int>(
                name: "AttackerUnitId",
                table: "Attacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpponentUnitId",
                table: "Attacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_AttackerUnitId",
                table: "Attacks",
                column: "AttackerUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_OpponentUnitId",
                table: "Attacks",
                column: "OpponentUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attacks_UserUnits_AttackerUnitId",
                table: "Attacks",
                column: "AttackerUnitId",
                principalTable: "UserUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attacks_UserUnits_OpponentUnitId",
                table: "Attacks",
                column: "OpponentUnitId",
                principalTable: "UserUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attacks_UserUnits_AttackerUnitId",
                table: "Attacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Attacks_UserUnits_OpponentUnitId",
                table: "Attacks");

            migrationBuilder.DropIndex(
                name: "IX_Attacks_AttackerUnitId",
                table: "Attacks");

            migrationBuilder.DropIndex(
                name: "IX_Attacks_OpponentUnitId",
                table: "Attacks");

            migrationBuilder.DropColumn(
                name: "AttackerUnitId",
                table: "Attacks");

            migrationBuilder.DropColumn(
                name: "OpponentUnitId",
                table: "Attacks");

            migrationBuilder.RenameColumn(
                name: "Round",
                table: "Attacks",
                newName: "IsDamageForAttacker");
        }
    }
}
