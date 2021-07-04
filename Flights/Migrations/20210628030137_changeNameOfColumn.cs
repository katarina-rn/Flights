using Microsoft.EntityFrameworkCore.Migrations;

namespace Flights.Migrations
{
    public partial class changeNameOfColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationFrom",
                table: "Flight",
                newName: "DestinationFromId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationFromId",
                table: "Flight",
                newName: "DestinationFrom");
        }
    }
}
