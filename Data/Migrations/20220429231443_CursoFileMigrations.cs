using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repaso_Net.Data.Migrations
{
    public partial class CursoFileMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "archivo",
                table: "cursos",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fileBase64",
                table: "cursos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombrefile",
                table: "cursos",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archivo",
                table: "cursos");

            migrationBuilder.DropColumn(
                name: "fileBase64",
                table: "cursos");

            migrationBuilder.DropColumn(
                name: "nombrefile",
                table: "cursos");
        }
    }
}
