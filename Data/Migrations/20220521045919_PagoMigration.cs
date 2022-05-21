using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Repaso_Net.Data.Migrations
{
    public partial class PagoMigration : Migration
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
                    monto = table.Column<double>(type: "double precision", nullable: false),
                    usuarioId = table.Column<string>(type: "text", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true),
                    metodoPago = table.Column<string>(type: "text", nullable: true),
                    banco = table.Column<string>(type: "text", nullable: true),
                    nombreCuenta = table.Column<string>(type: "text", nullable: true),
                    nombrefile = table.Column<string>(type: "text", nullable: true),
                    fileBase64 = table.Column<string>(type: "text", nullable: true),
                    archivo = table.Column<byte[]>(type: "bytea", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_DataPagos_usuarioId",
                table: "DataPagos",
                column: "usuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPagos");
        }
    }
}
