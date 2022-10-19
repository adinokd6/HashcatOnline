using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHash.Migrations
{
    public partial class AddHashTypeForHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HashType",
                table: "Hashes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashType",
                table: "Hashes");
        }
    }
}
