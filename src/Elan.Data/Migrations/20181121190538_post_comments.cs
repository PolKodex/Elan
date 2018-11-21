using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class post_comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasePostId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BasePostId",
                table: "Posts",
                column: "BasePostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_BasePostId",
                table: "Posts",
                column: "BasePostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_BasePostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_BasePostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BasePostId",
                table: "Posts");
        }
    }
}
