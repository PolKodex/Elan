using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class add_visibility_to_post_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisibilitySetting",
                table: "Posts",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisibilitySetting",
                table: "Posts");
        }
    }
}
