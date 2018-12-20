using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class friend_invitation_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendsInvitations",
                table: "FriendsInvitations");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FriendsInvitations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "FriendsInvitations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "FriendsInvitations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendsInvitations",
                table: "FriendsInvitations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendsInvitations_UserFromId",
                table: "FriendsInvitations",
                column: "UserFromId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendsInvitations",
                table: "FriendsInvitations");

            migrationBuilder.DropIndex(
                name: "IX_FriendsInvitations_UserFromId",
                table: "FriendsInvitations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FriendsInvitations");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "FriendsInvitations");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "FriendsInvitations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendsInvitations",
                table: "FriendsInvitations",
                columns: new[] { "UserFromId", "UserToId" });
        }
    }
}
