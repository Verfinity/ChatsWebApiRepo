using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatsWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Description_Field_From_Chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Chats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
