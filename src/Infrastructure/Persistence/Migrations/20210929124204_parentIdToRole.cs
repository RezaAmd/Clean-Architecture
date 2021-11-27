using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class parentIdToRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ParentId",
                table: "Roles",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Roles_ParentId",
                table: "Roles",
                column: "ParentId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Roles_ParentId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ParentId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Roles");
        }
    }
}
