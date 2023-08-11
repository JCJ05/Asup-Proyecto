using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repaso_Net.Migrations
{
    public partial class SqlServerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    apellidos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    identificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_tarjeta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDateYYMM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cvv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreTitular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_tarjeta", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_curso",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    horario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cupo = table.Column<int>(type: "int", nullable: false),
                    usuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    informacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    nombrefile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    archivo = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_curso", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_curso_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "t_pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    usuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NombreTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modalidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileVoucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    voucher = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_pago", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_pago_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "t_carrito",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    cursoId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_carrito", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_carrito_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_t_carrito_t_curso_cursoId",
                        column: x => x.cursoId,
                        principalTable: "t_curso",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_curso_alumno",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CursoId = table.Column<int>(type: "int", nullable: true),
                    fechaMatricula = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_curso_alumno", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_curso_alumno_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_t_curso_alumno_t_curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "t_curso",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_modulo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cursoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_modulo", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_modulo_t_curso_cursoId",
                        column: x => x.cursoId,
                        principalTable: "t_curso",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_compra",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PagoId = table.Column<int>(type: "int", nullable: true),
                    boleta = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_compra", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_compra_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_t_compra_t_pago_PagoId",
                        column: x => x.PagoId,
                        principalTable: "t_pago",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_seccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subtitulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    linkClase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    moduleid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_seccion", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_seccion_t_modulo_moduleid",
                        column: x => x.moduleid,
                        principalTable: "t_modulo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_detalle_compra",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cursoId = table.Column<int>(type: "int", nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    compraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_detalle_compra", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_detalle_compra_t_compra_compraId",
                        column: x => x.compraId,
                        principalTable: "t_compra",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_t_detalle_compra_t_curso_cursoId",
                        column: x => x.cursoId,
                        principalTable: "t_curso",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "t_archivo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    archivo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    seccionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_archivo", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_archivo_t_seccion_seccionId",
                        column: x => x.seccionId,
                        principalTable: "t_seccion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_t_archivo_seccionId",
                table: "t_archivo",
                column: "seccionId");

            migrationBuilder.CreateIndex(
                name: "IX_t_carrito_cursoId",
                table: "t_carrito",
                column: "cursoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_carrito_usuarioId",
                table: "t_carrito",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_t_compra_PagoId",
                table: "t_compra",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_compra_usuarioId",
                table: "t_compra",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_t_curso_usuarioId",
                table: "t_curso",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_t_curso_alumno_CursoId",
                table: "t_curso_alumno",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_curso_alumno_usuarioId",
                table: "t_curso_alumno",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_t_detalle_compra_compraId",
                table: "t_detalle_compra",
                column: "compraId");

            migrationBuilder.CreateIndex(
                name: "IX_t_detalle_compra_cursoId",
                table: "t_detalle_compra",
                column: "cursoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_modulo_cursoId",
                table: "t_modulo",
                column: "cursoId");

            migrationBuilder.CreateIndex(
                name: "IX_t_pago_usuarioId",
                table: "t_pago",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_t_seccion_moduleid",
                table: "t_seccion",
                column: "moduleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "t_archivo");

            migrationBuilder.DropTable(
                name: "t_carrito");

            migrationBuilder.DropTable(
                name: "t_curso_alumno");

            migrationBuilder.DropTable(
                name: "t_detalle_compra");

            migrationBuilder.DropTable(
                name: "t_tarjeta");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "t_seccion");

            migrationBuilder.DropTable(
                name: "t_compra");

            migrationBuilder.DropTable(
                name: "t_modulo");

            migrationBuilder.DropTable(
                name: "t_pago");

            migrationBuilder.DropTable(
                name: "t_curso");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
