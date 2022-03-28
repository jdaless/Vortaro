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
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Nomo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lingvoj", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzantoj",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Nomo = table.Column<string>(type: "TEXT", nullable: false),
                    Bildo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzantoj", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vortoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Teksto = table.Column<string>(type: "TEXT", nullable: true),
                    FinaĵoId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vortoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vortoj_Vortoj_FinaĵoId",
                        column: x => x.FinaĵoId,
                        principalTable: "Vortoj",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fontoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ĈuUzantkreita = table.Column<bool>(type: "INTEGER", nullable: false),
                    Favoreco = table.Column<int>(type: "INTEGER", nullable: false),
                    Ligilo = table.Column<string>(type: "TEXT", nullable: true),
                    Titolo = table.Column<string>(type: "TEXT", nullable: true),
                    KreintoId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fontoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fontoj_Uzantoj_KreintoId",
                        column: x => x.KreintoId,
                        principalTable: "Uzantoj",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Radiko",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RadikaVortoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DerivaĵaVortoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ordo = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Enhavoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FontoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VortoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Teksto = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    LingvoId = table.Column<string>(type: "TEXT", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Voĉdonoj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UzantoId = table.Column<string>(type: "TEXT", nullable: false),
                    EnhavoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ĈuSupraPoento = table.Column<bool>(type: "INTEGER", nullable: false),
                    FontoId = table.Column<Guid>(type: "TEXT", nullable: true)
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
                });

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
