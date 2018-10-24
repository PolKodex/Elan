using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class AddUserSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElanUserSetting",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Setting = table.Column<int>(nullable: false),
                    PrivacySetting = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElanUserSetting", x => new { x.UserId, x.Setting });
                    table.ForeignKey(
                        name: "FK_ElanUserSetting_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElanUserSetting");
        }
    }
}
