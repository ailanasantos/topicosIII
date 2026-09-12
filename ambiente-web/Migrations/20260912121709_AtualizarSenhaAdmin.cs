using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ambiente_web.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarSenhaAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "M1ni#Super2026!");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "123456");
        }
    }
}
