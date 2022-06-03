using Microsoft.EntityFrameworkCore.Migrations;

namespace Repaso_Net.Data.Migrations
{
    public partial class fieldLinkclaseMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "linkClase",
                table: "t_seccion",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "linkClase",
                table: "t_seccion");
        }
    }
}
