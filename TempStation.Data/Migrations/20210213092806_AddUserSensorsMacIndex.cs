using Microsoft.EntityFrameworkCore.Migrations;

namespace TempStation.Migrations
{
    public partial class AddUserSensorsMacIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserSensors_MacAddress",
                table: "UserSensors",
                column: "MacAddress",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSensors_MacAddress",
                table: "UserSensors");
        }
    }
}
