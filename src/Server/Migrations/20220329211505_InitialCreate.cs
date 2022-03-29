using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vortaro.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lingvoj",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_esperanto_ci"),
                    Nomo = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_esperanto_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lingvoj", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateTable(
                name: "Uzantoj",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_esperanto_ci"),
                    Nomo = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_esperanto_ci"),
                    Bildo = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_esperanto_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzantoj", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateTable(
                name: "Vortoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Teksto = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_esperanto_ci"),
                    FinaĵoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vortoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vortoj_Vortoj_FinaĵoId",
                        column: x => x.FinaĵoId,
                        principalTable: "Vortoj",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateTable(
                name: "Fontoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ĈuUzantkreita = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Favoreco = table.Column<int>(type: "int", nullable: false),
                    Ligilo = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_esperanto_ci"),
                    Titolo = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_esperanto_ci"),
                    KreintoId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_esperanto_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fontoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fontoj_Uzantoj_KreintoId",
                        column: x => x.KreintoId,
                        principalTable: "Uzantoj",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateTable(
                name: "Radiko",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RadikaVortoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DerivaĵaVortoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ordo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Radiko", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Radiko_Vortoj_DerivaĵaVortoId",
                        column: x => x.DerivaĵaVortoId,
                        principalTable: "Vortoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Radiko_Vortoj_RadikaVortoId",
                        column: x => x.RadikaVortoId,
                        principalTable: "Vortoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateTable(
                name: "Enhavoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FontoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VortoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Teksto = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_esperanto_ci"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_esperanto_ci"),
                    LingvoId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_esperanto_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enhavoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enhavoj_Fontoj_FontoId",
                        column: x => x.FontoId,
                        principalTable: "Fontoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enhavoj_Lingvoj_LingvoId",
                        column: x => x.LingvoId,
                        principalTable: "Lingvoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enhavoj_Vortoj_VortoId",
                        column: x => x.VortoId,
                        principalTable: "Vortoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateTable(
                name: "Voĉdonoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UzantoId = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_esperanto_ci"),
                    EnhavoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ĈuSupraPoento = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FontoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voĉdonoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voĉdonoj_Enhavoj_EnhavoId",
                        column: x => x.EnhavoId,
                        principalTable: "Enhavoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voĉdonoj_Fontoj_FontoId",
                        column: x => x.FontoId,
                        principalTable: "Fontoj",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Voĉdonoj_Uzantoj_UzantoId",
                        column: x => x.UzantoId,
                        principalTable: "Uzantoj",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_esperanto_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Enhavoj_FontoId",
                table: "Enhavoj",
                column: "FontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Enhavoj_LingvoId",
                table: "Enhavoj",
                column: "LingvoId");

            migrationBuilder.CreateIndex(
                name: "IX_Enhavoj_VortoId",
                table: "Enhavoj",
                column: "VortoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fontoj_KreintoId",
                table: "Fontoj",
                column: "KreintoId");

            migrationBuilder.CreateIndex(
                name: "IX_Radiko_DerivaĵaVortoId",
                table: "Radiko",
                column: "DerivaĵaVortoId");

            migrationBuilder.CreateIndex(
                name: "IX_Radiko_RadikaVortoId",
                table: "Radiko",
                column: "RadikaVortoId");

            migrationBuilder.CreateIndex(
                name: "IX_Voĉdonoj_EnhavoId",
                table: "Voĉdonoj",
                column: "EnhavoId");

            migrationBuilder.CreateIndex(
                name: "IX_Voĉdonoj_FontoId",
                table: "Voĉdonoj",
                column: "FontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Voĉdonoj_UzantoId",
                table: "Voĉdonoj",
                column: "UzantoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vortoj_FinaĵoId",
                table: "Vortoj",
                column: "FinaĵoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Radiko");

            migrationBuilder.DropTable(
                name: "Voĉdonoj");

            migrationBuilder.DropTable(
                name: "Enhavoj");

            migrationBuilder.DropTable(
                name: "Fontoj");

            migrationBuilder.DropTable(
                name: "Lingvoj");

            migrationBuilder.DropTable(
                name: "Vortoj");

            migrationBuilder.DropTable(
                name: "Uzantoj");
        }
    }
}
