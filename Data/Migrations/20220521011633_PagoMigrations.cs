using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Repaso_Net.Data.Migrations
{
    public partial class PagoMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataPagos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaPago = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    monto = table.Column<decimal>(type: "numeric", nullable: false),
                    usuarioId = table.Column<string>(type: "text", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPagos", x => x.id);
                    table.ForeignKey(
                        name: "FK_DataPagos_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataPagoCursos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pagoId = table.Column<int>(type: "integer", nullable: true),
                    cursoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPagoCursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_DataPagoCursos_cursos_cursoId",
                        column: x => x.cursoId,
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataPagoCursos_DataPagos_pagoId",
                        column: x => x.pagoId,
                        principalTable: "DataPagos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataPagoCursos_cursoId",
                table: "DataPagoCursos",
                column: "cursoId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPagoCursos_pagoId",
                table: "DataPagoCursos",
                column: "pagoId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPagos_usuarioId",
                table: "DataPagos",
                column: "usuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPagoCursos");

            migrationBuilder.DropTable(
                name: "DataPagos");
        }
    }
}
