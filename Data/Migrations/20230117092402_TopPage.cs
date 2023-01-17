using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GibiSu.Data.Migrations
{
    public partial class TopPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TopPageUrl",
                table: "Pages",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TopPageUrl",
                table: "Pages",
                column: "TopPageUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Pages_TopPageUrl",
                table: "Pages",
                column: "TopPageUrl",
                principalTable: "Pages",
                principalColumn: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Pages_TopPageUrl",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_TopPageUrl",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "TopPageUrl",
                table: "Pages");
        }
    }
}
