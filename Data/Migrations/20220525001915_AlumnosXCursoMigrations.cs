using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Repaso_Net.Data.Migrations
{
    public partial class AlumnosXCursoMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_curso_alumno",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuarioId = table.Column<string>(type: "text", nullable: true),
                    CursoId = table.Column<int>(type: "integer", nullable: true),
                    fechaMatricula = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_curso_alumno", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_curso_alumno_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t_curso_alumno_t_curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "t_curso",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_curso_alumno_CursoId",
                table: "t_curso_alumno",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_curso_alumno_usuarioId",
                table: "t_curso_alumno",
                column: "usuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_curso_alumno");
        }
    }
}
