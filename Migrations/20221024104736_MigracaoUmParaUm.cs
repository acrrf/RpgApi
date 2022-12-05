using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    public partial class MigracaoUmParaUm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonagemId",
                table: "Armas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "PersonagemId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 2,
                column: "PersonagemId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 3,
                column: "PersonagemId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 4,
                column: "PersonagemId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 5,
                column: "PersonagemId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 6,
                column: "PersonagemId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 7,
                column: "PersonagemId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 60, 146, 245, 218, 237, 89, 124, 199, 37, 82, 153, 10, 193, 233, 229, 11, 121, 66, 195, 34, 111, 175, 171, 148, 98, 122, 29, 102, 204, 249, 152, 136, 11, 129, 135, 222, 157, 49, 238, 204, 146, 28, 154, 184, 68, 143, 44, 48, 252, 236, 56, 44, 111, 123, 243, 241, 134, 140, 94, 193, 60, 83, 121, 42 }, new byte[] { 213, 117, 68, 94, 231, 95, 120, 231, 21, 136, 8, 123, 128, 102, 223, 52, 69, 203, 125, 16, 163, 185, 66, 17, 98, 201, 142, 240, 166, 73, 140, 247, 244, 68, 51, 109, 115, 26, 172, 53, 87, 116, 237, 7, 201, 28, 21, 203, 170, 131, 92, 241, 173, 91, 62, 20, 231, 25, 235, 156, 86, 240, 75, 108, 229, 65, 205, 188, 10, 21, 41, 58, 121, 87, 249, 112, 83, 153, 50, 58, 143, 31, 120, 123, 63, 151, 3, 47, 177, 117, 63, 233, 70, 48, 157, 33, 51, 22, 54, 50, 142, 239, 253, 142, 177, 20, 145, 19, 130, 117, 78, 212, 216, 145, 161, 245, 179, 4, 113, 144, 90, 83, 52, 234, 127, 165, 28, 191 } });

            migrationBuilder.CreateIndex(
                name: "IX_Armas_PersonagemId",
                table: "Armas",
                column: "PersonagemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Armas_Personagens_PersonagemId",
                table: "Armas",
                column: "PersonagemId",
                principalTable: "Personagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armas_Personagens_PersonagemId",
                table: "Armas");

            migrationBuilder.DropIndex(
                name: "IX_Armas_PersonagemId",
                table: "Armas");

            migrationBuilder.DropColumn(
                name: "PersonagemId",
                table: "Armas");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 112, 9, 54, 56, 46, 26, 54, 176, 83, 227, 60, 218, 106, 95, 145, 144, 20, 31, 50, 3, 46, 203, 12, 18, 206, 255, 182, 69, 136, 150, 207, 115, 53, 237, 207, 121, 73, 217, 116, 102, 208, 148, 101, 96, 155, 224, 86, 43, 89, 100, 237, 124, 137, 71, 87, 85, 109, 80, 12, 51, 19, 118, 166, 6 }, new byte[] { 20, 129, 183, 245, 236, 76, 147, 147, 250, 132, 167, 37, 226, 12, 57, 247, 62, 160, 126, 114, 171, 205, 125, 6, 98, 35, 47, 2, 130, 239, 214, 57, 242, 79, 224, 60, 27, 62, 105, 156, 98, 102, 83, 5, 199, 28, 174, 83, 240, 206, 23, 144, 192, 224, 60, 65, 135, 110, 218, 7, 86, 24, 72, 210, 24, 224, 28, 5, 33, 122, 114, 248, 24, 63, 136, 229, 12, 38, 198, 106, 219, 209, 215, 162, 225, 161, 163, 174, 224, 246, 89, 146, 111, 148, 230, 217, 79, 203, 8, 90, 137, 110, 174, 167, 104, 54, 102, 121, 81, 121, 171, 79, 211, 55, 231, 0, 235, 101, 45, 231, 206, 69, 41, 12, 163, 20, 150, 83 } });
        }
    }
}
