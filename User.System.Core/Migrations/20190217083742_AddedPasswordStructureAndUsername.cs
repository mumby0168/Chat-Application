using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User.System.Core.Migrations
{
    public partial class AddedPasswordStructureAndUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SocketUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "SocketUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "SocketUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "SocketUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "SocketUsers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "SocketUsers");
        }
    }
}
