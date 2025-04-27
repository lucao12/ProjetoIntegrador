using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoIntegrador.Migrations
{
    /// <inheritdoc />
    public partial class updateTabelaLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NutriId",
                table: "Layout",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Layout_NutriId",
                table: "Layout",
                column: "NutriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Layout_Usuarios_NutriId",
                table: "Layout",
                column: "NutriId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Layout_Usuarios_NutriId",
                table: "Layout");

            migrationBuilder.DropIndex(
                name: "IX_Layout_NutriId",
                table: "Layout");

            migrationBuilder.DropColumn(
                name: "NutriId",
                table: "Layout");
        }
    }
}
