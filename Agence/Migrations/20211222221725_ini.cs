using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Agence.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entreprise",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entreprise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NOrdre = table.Column<int>(nullable: false),
                    Annee = table.Column<int>(nullable: false),
                    NAffaire = table.Column<string>(maxLength: 12, nullable: true),
                    NomClient = table.Column<string>(maxLength: 200, nullable: true),
                    AdresseClient = table.Column<string>(maxLength: 200, nullable: true),
                    NatureId = table.Column<int>(nullable: true),
                    DatePaiement = table.Column<DateTime>(nullable: true),
                    DateReception = table.Column<DateTime>(nullable: true),
                    EntrepriseId = table.Column<int>(nullable: true),
                    DateAffectation = table.Column<DateTime>(nullable: true),
                    DateRemise = table.Column<DateTime>(nullable: true),
                    Realise = table.Column<bool>(nullable: false),
                    Observation = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registres_Entreprise_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registres_Nature_NatureId",
                        column: x => x.NatureId,
                        principalTable: "Nature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registres_EntrepriseId",
                table: "Registres",
                column: "EntrepriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Registres_NAffaire",
                table: "Registres",
                column: "NAffaire",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registres_NatureId",
                table: "Registres",
                column: "NatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registres");

            migrationBuilder.DropTable(
                name: "Entreprise");

            migrationBuilder.DropTable(
                name: "Nature");
        }
    }
}
