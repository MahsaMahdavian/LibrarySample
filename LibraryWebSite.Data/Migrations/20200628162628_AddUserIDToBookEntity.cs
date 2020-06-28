using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryWebSite.Data.Migrations
{
    public partial class AddUserIDToBookEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Books");
        }
    }
}
