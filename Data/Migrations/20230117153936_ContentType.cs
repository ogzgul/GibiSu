using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GibiSu.Data.Migrations
{
    public partial class ContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Banner",
                table: "Pages",
                type: "image",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "image");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Contents",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Contents");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Banner",
                table: "Pages",
                type: "image",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "image",
                oldNullable: true);
        }
    }
}
