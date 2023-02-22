using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.Migrations
{
    public partial class FixedDependencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProductId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProductionRoomId",
                table: "Contracts");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProductId",
                table: "Contracts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProductionRoomId",
                table: "Contracts",
                column: "ProductionRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProductId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProductionRoomId",
                table: "Contracts");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProductId",
                table: "Contracts",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProductionRoomId",
                table: "Contracts",
                column: "ProductionRoomId",
                unique: true);
        }
    }
}
