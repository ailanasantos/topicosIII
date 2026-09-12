using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ambiente_web.Migrations
{
    /// <inheritdoc />
    public partial class RemoverSeedUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "Email", "Nome", "Perfil", "Senha" },
                values: new object[] { 1, true, "admin@minisuper.com", "Administrador", "Administrador", "M1ni#Super2026!" });
        }
    }
}
