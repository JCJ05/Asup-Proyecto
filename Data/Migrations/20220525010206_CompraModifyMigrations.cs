using Microsoft.EntityFrameworkCore.Migrations;

namespace Repaso_Net.Data.Migrations
{
    public partial class CompraModifyMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "t_compra",
                newName: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_t_compra_usuarioId",
                table: "t_compra",
                column: "usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_compra_AspNetUsers_usuarioId",
                table: "t_compra",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_compra_AspNetUsers_usuarioId",
                table: "t_compra");

            migrationBuilder.DropIndex(
                name: "IX_t_compra_usuarioId",
                table: "t_compra");

            migrationBuilder.RenameColumn(
                name: "usuarioId",
                table: "t_compra",
                newName: "UserID");
        }
    }
}
