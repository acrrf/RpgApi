using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    public partial class MigracaoMuitosParaMuitos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Derrotas",
                table: "Personagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Disputas",
                table: "Personagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Vitorias",
                table: "Personagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Habilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habilidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonagemHabilidades",
                columns: table => new
                {
                    PersonagemId = table.Column<int>(type: "int", nullable: false),
                    HabilidadeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagemHabilidades", x => new { x.PersonagemId, x.HabilidadeId });
                    table.ForeignKey(
                        name: "FK_PersonagemHabilidades_Habilidades_HabilidadeId",
                        column: x => x.HabilidadeId,
                        principalTable: "Habilidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonagemHabilidades_Personagens_PersonagemId",
                        column: x => x.PersonagemId,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Habilidades",
                columns: new[] { "Id", "Dano", "Nome" },
                values: new object[,]
                {
                    { 1, 39, "Adormecer" },
                    { 2, 41, "Congelar" },
                    { 3, 37, "Hipnotizar" }
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 92, 162, 145, 233, 136, 234, 166, 63, 187, 158, 10, 29, 79, 181, 147, 235, 237, 206, 228, 173, 226, 149, 193, 125, 54, 244, 27, 142, 130, 173, 150, 249, 156, 67, 44, 184, 99, 223, 72, 220, 183, 20, 45, 31, 92, 189, 51, 131, 44, 230, 165, 228, 214, 149, 151, 217, 82, 133, 53, 149, 116, 177, 193, 124 }, new byte[] { 195, 115, 220, 171, 57, 18, 78, 52, 148, 90, 197, 160, 89, 145, 123, 226, 84, 106, 216, 11, 207, 11, 34, 42, 183, 1, 63, 105, 144, 141, 98, 137, 147, 124, 140, 7, 12, 237, 124, 141, 221, 18, 252, 127, 162, 61, 123, 106, 9, 174, 149, 29, 56, 180, 217, 93, 244, 37, 200, 19, 40, 162, 48, 1, 133, 212, 99, 187, 127, 21, 69, 8, 77, 1, 125, 250, 14, 168, 45, 54, 254, 187, 59, 11, 31, 51, 145, 46, 251, 246, 162, 182, 28, 64, 3, 134, 213, 246, 180, 6, 30, 78, 2, 167, 205, 94, 125, 87, 195, 37, 177, 95, 234, 159, 249, 243, 199, 0, 66, 186, 57, 90, 230, 245, 132, 134, 247, 170 } });

            migrationBuilder.InsertData(
                table: "PersonagemHabilidades",
                columns: new[] { "HabilidadeId", "PersonagemId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 3 },
                    { 3, 4 },
                    { 1, 5 },
                    { 2, 6 },
                    { 3, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonagemHabilidades_HabilidadeId",
                table: "PersonagemHabilidades",
                column: "HabilidadeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonagemHabilidades");

            migrationBuilder.DropTable(
                name: "Habilidades");

            migrationBuilder.DropColumn(
                name: "Derrotas",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Disputas",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Vitorias",
                table: "Personagens");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 60, 146, 245, 218, 237, 89, 124, 199, 37, 82, 153, 10, 193, 233, 229, 11, 121, 66, 195, 34, 111, 175, 171, 148, 98, 122, 29, 102, 204, 249, 152, 136, 11, 129, 135, 222, 157, 49, 238, 204, 146, 28, 154, 184, 68, 143, 44, 48, 252, 236, 56, 44, 111, 123, 243, 241, 134, 140, 94, 193, 60, 83, 121, 42 }, new byte[] { 213, 117, 68, 94, 231, 95, 120, 231, 21, 136, 8, 123, 128, 102, 223, 52, 69, 203, 125, 16, 163, 185, 66, 17, 98, 201, 142, 240, 166, 73, 140, 247, 244, 68, 51, 109, 115, 26, 172, 53, 87, 116, 237, 7, 201, 28, 21, 203, 170, 131, 92, 241, 173, 91, 62, 20, 231, 25, 235, 156, 86, 240, 75, 108, 229, 65, 205, 188, 10, 21, 41, 58, 121, 87, 249, 112, 83, 153, 50, 58, 143, 31, 120, 123, 63, 151, 3, 47, 177, 117, 63, 233, 70, 48, 157, 33, 51, 22, 54, 50, 142, 239, 253, 142, 177, 20, 145, 19, 130, 117, 78, 212, 216, 145, 161, 245, 179, 4, 113, 144, 90, 83, 52, 234, 127, 165, 28, 191 } });
        }
    }
}
