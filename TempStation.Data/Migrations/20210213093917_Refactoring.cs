using Microsoft.EntityFrameworkCore.Migrations;

namespace TempStation.Migrations
{
    public partial class Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MainSensorTemperatures",
                table: "MainSensorTemperatures");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "MainSensorTemperatures");

            migrationBuilder.RenameTable(
                name: "MainSensorTemperatures",
                newName: "SensorTemperatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorTemperatures",
                table: "SensorTemperatures",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorTemperatures",
                table: "SensorTemperatures");

            migrationBuilder.RenameTable(
                name: "SensorTemperatures",
                newName: "MainSensorTemperatures");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "MainSensorTemperatures",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainSensorTemperatures",
                table: "MainSensorTemperatures",
                column: "Id");
        }
    }
}
