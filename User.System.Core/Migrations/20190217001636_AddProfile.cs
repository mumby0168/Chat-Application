using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User.System.Core.Migrations
{
    public partial class AddProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Identities",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Identities");

            migrationBuilder.RenameTable(
                name: "Identities",
                newName: "SocketUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "SocketUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocketUsers",
                table: "SocketUsers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SocketUsers",
                table: "SocketUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "SocketUsers");

            migrationBuilder.RenameTable(
                name: "SocketUsers",
                newName: "Identities");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Identities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Identities",
                table: "Identities",
                column: "Id");
        }
    }
}
