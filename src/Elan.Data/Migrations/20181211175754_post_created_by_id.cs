using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elan.Data.Migrations
{
    public partial class post_created_by_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
