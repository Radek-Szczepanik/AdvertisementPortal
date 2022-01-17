using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertisementPortal.Migrations
{
    public partial class AdvertisementUserIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CreatedById",
                table: "Advertisements",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Users_CreatedById",
                table: "Advertisements",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Users_CreatedById",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_CreatedById",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Advertisements");
        }
    }
}
