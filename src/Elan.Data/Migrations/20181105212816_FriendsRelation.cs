using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class FriendsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FirstUserId1",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_SecondUserId1",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_FirstUserId1",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_SecondUserId1",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "FirstUserId1",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "SecondUserId1",
                table: "Friends");

            migrationBuilder.AlterColumn<Guid>(
                name: "SecondUserId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<Guid>(
                name: "FirstUserId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Friends_SecondUserId",
                table: "Friends",
                column: "SecondUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FirstUserId",
                table: "Friends",
                column: "FirstUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_SecondUserId",
                table: "Friends",
                column: "SecondUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FirstUserId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_SecondUserId",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_SecondUserId",
                table: "Friends");

            migrationBuilder.AlterColumn<int>(
                name: "SecondUserId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<int>(
                name: "FirstUserId",
                table: "Friends",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "FirstUserId1",
                table: "Friends",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SecondUserId1",
                table: "Friends",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FirstUserId1",
                table: "Friends",
                column: "FirstUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_SecondUserId1",
                table: "Friends",
                column: "SecondUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FirstUserId1",
                table: "Friends",
                column: "FirstUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_SecondUserId1",
                table: "Friends",
                column: "SecondUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
