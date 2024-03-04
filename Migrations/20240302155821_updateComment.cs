using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEnterprise.Migrations
{
    public partial class updateComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContributionId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ContributionId",
                table: "Comments",
                column: "ContributionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Contributions_ContributionId",
                table: "Comments",
                column: "ContributionId",
                principalTable: "Contributions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Contributions_ContributionId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ContributionId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ContributionId",
                table: "Comments");
        }
    }
}
