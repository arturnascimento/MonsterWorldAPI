using Microsoft.EntityFrameworkCore.Migrations;

namespace MonsterWorldApi.Migrations
{
    public partial class createdupdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Monster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Monster",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Monster");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Monster");
        }
    }
}
