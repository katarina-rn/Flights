using Microsoft.EntityFrameworkCore.Migrations;

namespace Flights.Migrations
{
    public partial class addChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_DestinationToId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DestinationToId",
                table: "Flights");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationToId",
                table: "Flights",
                column: "DestinationToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_DestinationToId",
                table: "Flights",
                column: "DestinationToId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
