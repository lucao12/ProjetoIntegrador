using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoIntegrador.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Calorias = table.Column<int>(type: "INTEGER", nullable: true),
                    Carboidratos = table.Column<int>(type: "INTEGER", nullable: true),
                    Proteinas = table.Column<int>(type: "INTEGER", nullable: true),
                    Gorduras = table.Column<int>(type: "INTEGER", nullable: true),
                    VitaminasEMinerais = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    Sexo = table.Column<string>(type: "TEXT", nullable: true),
                    Idade = table.Column<int>(type: "INTEGER", nullable: true),
                    Peso = table.Column<double>(type: "REAL", nullable: true),
                    Altura = table.Column<double>(type: "REAL", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
                    Instagram = table.Column<string>(type: "TEXT", nullable: true),
                    Foto = table.Column<string>(type: "TEXT", nullable: true),
                    Hash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdmCriou",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Criou = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmCriou", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmCriou_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlimentosUsuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlimentoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlimentosUsuarios", x => new { x.UsuarioId, x.AlimentoId });
                    table.ForeignKey(
                        name: "FK_AlimentosUsuarios_Alimentos_AlimentoId",
                        column: x => x.AlimentoId,
                        principalTable: "Alimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlimentosUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dieta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    NutricionistaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dieta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dieta_Usuarios_NutricionistaId",
                        column: x => x.NutricionistaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dieta_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    NutricionistaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mensage = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensagem_Usuarios_NutricionistaId",
                        column: x => x.NutricionistaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mensagem_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    NutricionistaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Aceito = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_NutricionistaId",
                        column: x => x.NutricionistaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlimentoQuantidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlimentosId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    DietaId = table.Column<int>(type: "INTEGER", nullable: true),
                    DietaId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    DietaId2 = table.Column<int>(type: "INTEGER", nullable: true),
                    DietaId3 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlimentoQuantidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlimentoQuantidade_Alimentos_AlimentosId",
                        column: x => x.AlimentosId,
                        principalTable: "Alimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlimentoQuantidade_Dieta_DietaId",
                        column: x => x.DietaId,
                        principalTable: "Dieta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlimentoQuantidade_Dieta_DietaId1",
                        column: x => x.DietaId1,
                        principalTable: "Dieta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlimentoQuantidade_Dieta_DietaId2",
                        column: x => x.DietaId2,
                        principalTable: "Dieta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlimentoQuantidade_Dieta_DietaId3",
                        column: x => x.DietaId3,
                        principalTable: "Dieta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmCriou_UserId",
                table: "AdmCriou",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoQuantidade_AlimentosId",
                table: "AlimentoQuantidade",
                column: "AlimentosId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoQuantidade_DietaId",
                table: "AlimentoQuantidade",
                column: "DietaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoQuantidade_DietaId1",
                table: "AlimentoQuantidade",
                column: "DietaId1");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoQuantidade_DietaId2",
                table: "AlimentoQuantidade",
                column: "DietaId2");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentoQuantidade_DietaId3",
                table: "AlimentoQuantidade",
                column: "DietaId3");

            migrationBuilder.CreateIndex(
                name: "IX_AlimentosUsuarios_AlimentoId",
                table: "AlimentosUsuarios",
                column: "AlimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Dieta_NutricionistaId",
                table: "Dieta",
                column: "NutricionistaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dieta_UsuarioId",
                table: "Dieta",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagem_NutricionistaId",
                table: "Mensagem",
                column: "NutricionistaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagem_UsuarioId",
                table: "Mensagem",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_NutricionistaId",
                table: "Pedidos",
                column: "NutricionistaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmCriou");

            migrationBuilder.DropTable(
                name: "AlimentoQuantidade");

            migrationBuilder.DropTable(
                name: "AlimentosUsuarios");

            migrationBuilder.DropTable(
                name: "Mensagem");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Dieta");

            migrationBuilder.DropTable(
                name: "Alimentos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
