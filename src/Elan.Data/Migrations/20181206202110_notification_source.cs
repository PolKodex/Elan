using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class notification_source : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VisibilitySetting",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "SourceId",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "VisibilitySetting",
                table: "Posts",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
