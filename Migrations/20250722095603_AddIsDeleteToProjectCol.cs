using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio_builder_server.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeleteToProjectCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "IsDelete",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Projects");
        }
    }
}
