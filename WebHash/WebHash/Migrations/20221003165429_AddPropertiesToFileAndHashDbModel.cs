using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHash.Migrations
{
    public partial class AddPropertiesToFileAndHashDbModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Hashes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CrackedPasswords",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CrackingTime",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsResultFile",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UnCrackedPasswords",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Hashes");

            migrationBuilder.DropColumn(
                name: "CrackedPasswords",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CrackingTime",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsResultFile",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "UnCrackedPasswords",
                table: "Files");
        }
    }
}
