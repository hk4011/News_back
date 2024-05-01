using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorsId",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorsId",
                table: "News",
                column: "AuthorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Authors_AuthorsId",
                table: "News",
                column: "AuthorsId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Authors_AuthorsId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_AuthorsId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "AuthorsId",
                table: "News");
        }
    }
}
