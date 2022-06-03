using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Repaso_Net.Data.Migrations
{
    public partial class SeccionModulosMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_modulo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    cursoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_modulo", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_modulo_t_curso_cursoId",
                        column: x => x.cursoId,
                        principalTable: "t_curso",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_seccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(type: "text", nullable: true),
                    subtitulo = table.Column<string>(type: "text", nullable: true),
                    moduleid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_seccion", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_seccion_t_modulo_moduleid",
                        column: x => x.moduleid,
                        principalTable: "t_modulo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_archivo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombreArchivo = table.Column<string>(type: "text", nullable: true),
                    extension = table.Column<string>(type: "text", nullable: true),
                    archivo = table.Column<byte[]>(type: "bytea", nullable: true),
                    seccionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_archivo", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_archivo_t_seccion_seccionId",
                        column: x => x.seccionId,
                        principalTable: "t_seccion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_archivo_seccionId",
                table: "t_archivo",
                column: "seccionId");

            migrationBuilder.CreateIndex(
                name: "IX_t_modulo_cursoId",
                table: "t_modulo",
                column: "cursoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_seccion_moduleid",
                table: "t_seccion",
                column: "moduleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_archivo");

            migrationBuilder.DropTable(
                name: "t_seccion");

            migrationBuilder.DropTable(
                name: "t_modulo");
        }
    }
}
