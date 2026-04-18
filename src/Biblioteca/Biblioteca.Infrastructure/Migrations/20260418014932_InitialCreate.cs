using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_BIBLIOTECA",
                columns: table => new
                {
                    IdBiblioteca = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_BIBLIOTECA", x => x.IdBiblioteca);
                });

            migrationBuilder.CreateTable(
                name: "T_GENERO",
                columns: table => new
                {
                    IdGenero = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GENERO", x => x.IdGenero);
                });

            migrationBuilder.CreateTable(
                name: "T_CLIENTE",
                columns: table => new
                {
                    IdCliente = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DataNascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Salt = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    IdBiblioteca = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLIENTE", x => x.IdCliente);
                    table.ForeignKey(
                        name: "FK_T_CLIENTE_T_BIBLIOTECA_IdBiblioteca",
                        column: x => x.IdBiblioteca,
                        principalTable: "T_BIBLIOTECA",
                        principalColumn: "IdBiblioteca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_FUNCIONARIO",
                columns: table => new
                {
                    IdFuncionario = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DataNasc = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Salt = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Turno = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    IdBiblioteca = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FUNCIONARIO", x => x.IdFuncionario);
                    table.ForeignKey(
                        name: "FK_T_FUNCIONARIO_T_BIBLIOTECA_IdBiblioteca",
                        column: x => x.IdBiblioteca,
                        principalTable: "T_BIBLIOTECA",
                        principalColumn: "IdBiblioteca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_LIVRO",
                columns: table => new
                {
                    IdLivro = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Titulo = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Serie = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    DataLancamento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    NPaginas = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdBiblioteca = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_LIVRO", x => x.IdLivro);
                    table.ForeignKey(
                        name: "FK_T_LIVRO_T_BIBLIOTECA_IdBiblioteca",
                        column: x => x.IdBiblioteca,
                        principalTable: "T_BIBLIOTECA",
                        principalColumn: "IdBiblioteca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_EMPRESTIMO",
                columns: table => new
                {
                    IdEmprestimo = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IdCliente = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_EMPRESTIMO", x => x.IdEmprestimo);
                    table.ForeignKey(
                        name: "FK_T_EMPRESTIMO_T_CLIENTE_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "T_CLIENTE",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_LIVRO_GENERO",
                columns: table => new
                {
                    GenerosIdGenero = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    LivrosIdLivro = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_LIVRO_GENERO", x => new { x.GenerosIdGenero, x.LivrosIdLivro });
                    table.ForeignKey(
                        name: "FK_T_LIVRO_GENERO_T_GENERO_GenerosIdGenero",
                        column: x => x.GenerosIdGenero,
                        principalTable: "T_GENERO",
                        principalColumn: "IdGenero",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_LIVRO_GENERO_T_LIVRO_LivrosIdLivro",
                        column: x => x.LivrosIdLivro,
                        principalTable: "T_LIVRO",
                        principalColumn: "IdLivro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_LIVRO_EMPRESTIMO",
                columns: table => new
                {
                    EmprestimosIdEmprestimo = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    LivrosIdLivro = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_LIVRO_EMPRESTIMO", x => new { x.EmprestimosIdEmprestimo, x.LivrosIdLivro });
                    table.ForeignKey(
                        name: "FK_T_LIVRO_EMPRESTIMO_T_EMPRESTIMO_EmprestimosIdEmprestimo",
                        column: x => x.EmprestimosIdEmprestimo,
                        principalTable: "T_EMPRESTIMO",
                        principalColumn: "IdEmprestimo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_LIVRO_EMPRESTIMO_T_LIVRO_LivrosIdLivro",
                        column: x => x.LivrosIdLivro,
                        principalTable: "T_LIVRO",
                        principalColumn: "IdLivro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_CLIENTE_IdBiblioteca",
                table: "T_CLIENTE",
                column: "IdBiblioteca");

            migrationBuilder.CreateIndex(
                name: "IX_T_EMPRESTIMO_IdCliente",
                table: "T_EMPRESTIMO",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_T_FUNCIONARIO_IdBiblioteca",
                table: "T_FUNCIONARIO",
                column: "IdBiblioteca");

            migrationBuilder.CreateIndex(
                name: "IX_T_LIVRO_IdBiblioteca",
                table: "T_LIVRO",
                column: "IdBiblioteca");

            migrationBuilder.CreateIndex(
                name: "IX_T_LIVRO_EMPRESTIMO_LivrosIdLivro",
                table: "T_LIVRO_EMPRESTIMO",
                column: "LivrosIdLivro");

            migrationBuilder.CreateIndex(
                name: "IX_T_LIVRO_GENERO_LivrosIdLivro",
                table: "T_LIVRO_GENERO",
                column: "LivrosIdLivro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_FUNCIONARIO");

            migrationBuilder.DropTable(
                name: "T_LIVRO_EMPRESTIMO");

            migrationBuilder.DropTable(
                name: "T_LIVRO_GENERO");

            migrationBuilder.DropTable(
                name: "T_EMPRESTIMO");

            migrationBuilder.DropTable(
                name: "T_GENERO");

            migrationBuilder.DropTable(
                name: "T_LIVRO");

            migrationBuilder.DropTable(
                name: "T_CLIENTE");

            migrationBuilder.DropTable(
                name: "T_BIBLIOTECA");
        }
    }
}
