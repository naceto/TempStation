using Microsoft.EntityFrameworkCore.Migrations;

namespace TempStation.Migrations
{
    public partial class AddNameToUserSensors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TempStationUserId",
                table: "UserSensors",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MacAddress",
                table: "UserSensors",
                type: "TEXT",
                maxLength: 17,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 17,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserSensors",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserSensors");

            migrationBuilder.AlterColumn<string>(
                name: "TempStationUserId",
                table: "UserSensors",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "MacAddress",
                table: "UserSensors",
                type: "TEXT",
                maxLength: 17,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 17);
        }
    }
}
