using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class addIsDeleteinMegazine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Megazines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Megazines");
        }
    }
}
