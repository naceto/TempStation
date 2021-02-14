using Microsoft.EntityFrameworkCore.Migrations;

namespace TempStation.Migrations
{
    public partial class AddUserSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Temperatures",
                table: "Temperatures");

            migrationBuilder.RenameTable(
                name: "Temperatures",
                newName: "MainSensorTemperatures");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "MainSensorTemperatures",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "MainSensorTemperatures",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserSensorId",
                table: "MainSensorTemperatures",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainSensorTemperatures",
                table: "MainSensorTemperatures",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserSensors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TempStationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    MacAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSensors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainSensorTemperatures",
                table: "MainSensorTemperatures");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "MainSensorTemperatures");

            migrationBuilder.DropColumn(
                name: "UserSensorId",
                table: "MainSensorTemperatures");

            migrationBuilder.RenameTable(
                name: "MainSensorTemperatures",
                newName: "Temperatures");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Temperatures",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Temperatures",
                table: "Temperatures",
                column: "Id");
        }
    }
}
