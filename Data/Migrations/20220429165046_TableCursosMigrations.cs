using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Repaso_Net.Data.Migrations
{
    public partial class TableCursosMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cursos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    fechaInicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fechaFin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    horario = table.Column<string>(type: "text", nullable: true),
                    cupo = table.Column<int>(type: "integer", nullable: false),
                    usuarioId = table.Column<string>(type: "text", nullable: true),
                    informacion = table.Column<string>(type: "text", nullable: true),
                    precio = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_cursos_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cursos_usuarioId",
                table: "cursos",
                column: "usuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cursos");
        }
    }
}
