using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class chatmessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserFromId = table.Column<Guid>(nullable: true),
                    UserToId = table.Column<Guid>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    SentOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_UserToId",
                        column: x => x.UserToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserFromId",
                table: "ChatMessages",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserToId",
                table: "ChatMessages",
                column: "UserToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");
        }
    }
}
