using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoIntegrador.Migrations
{
    /// <inheritdoc />
    public partial class adicaoTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Layout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlimentoLayout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlimentosId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    LayoutId = table.Column<int>(type: "INTEGER", nullable: true),
                    LayoutId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    LayoutId2 = table.Column<int>(type: "INTEGER", nullable: true),
                    LayoutId3 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlimentoLayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlimentoLayout_Alimentos_AlimentosId",
                        column: x => x.AlimentosId,
                        principalTable: "Alimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlimentoLayout_Layout_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layout",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlimentoLayout_Layout_LayoutId1",
                        column: x => x.LayoutId1,
                        principalTable: "Layout",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlimentoLayout_Layout_LayoutId2",
                        column: x => x.LayoutId2,
                        principalTable: "Layout",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlimentoLayout_Layout_LayoutId3",
                        column: x => x.LayoutId3,
                        principalTable: "Layout",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoLayout_AlimentosId",
                table: "AlimentoLayout",
                column: "AlimentosId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoLayout_LayoutId",
                table: "AlimentoLayout",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoLayout_LayoutId1",
                table: "AlimentoLayout",
                column: "LayoutId1");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoLayout_LayoutId2",
                table: "AlimentoLayout",
                column: "LayoutId2");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoLayout_LayoutId3",
                table: "AlimentoLayout",
                column: "LayoutId3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlimentoLayout");

            migrationBuilder.DropTable(
                name: "Layout");
        }
    }
}
