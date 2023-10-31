using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PesoEstoque",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "QuantidadeEstoque",
                table: "Produto");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Produto",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Produto",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Produto",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Preco",
                table: "Produto",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TipoEstoque",
                table: "Produto",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Nome = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Telefone = table.Column<string>(type: "longtext", nullable: false),
                    CpfCnpj = table.Column<string>(type: "longtext", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Removido = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.FornecedorId);
                })
                .Annotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");

            migrationBuilder.CreateTable(
                name: "PedidoVenda",
                columns: table => new
                {
                    PedidoVendaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: true),
                    VendedorId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoVenda", x => x.PedidoVendaId);
                    table.ForeignKey(
                        name: "FK_PedidoVenda_Usuario_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");

            migrationBuilder.CreateTable(
                name: "PedidoCompra",
                columns: table => new
                {
                    PedidoCompraId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: true),
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CompradorId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoCompra", x => x.PedidoCompraId);
                    table.ForeignKey(
                        name: "FK_PedidoCompra_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "FornecedorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoCompra_Usuario_CompradorId",
                        column: x => x.CompradorId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");

            migrationBuilder.CreateTable(
                name: "PedidoVendaProduto",
                columns: table => new
                {
                    PedidoVendaProdutoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorPorQuantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valorDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valorAdicional = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PedidoVendaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoVendaProduto", x => x.PedidoVendaProdutoId);
                    table.ForeignKey(
                        name: "FK_PedidoVendaProduto_PedidoVenda_PedidoVendaId",
                        column: x => x.PedidoVendaId,
                        principalTable: "PedidoVenda",
                        principalColumn: "PedidoVendaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoVendaProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");

            migrationBuilder.CreateTable(
                name: "PedidoCompraProduto",
                columns: table => new
                {
                    PedidoCompraProdutoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorPorQuantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PedidoCompraId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoCompraProduto", x => x.PedidoCompraProdutoId);
                    table.ForeignKey(
                        name: "FK_PedidoCompraProduto_PedidoCompra_PedidoCompraId",
                        column: x => x.PedidoCompraId,
                        principalTable: "PedidoCompra",
                        principalColumn: "PedidoCompraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoCompraProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoCompra_CompradorId",
                table: "PedidoCompra",
                column: "CompradorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoCompra_FornecedorId",
                table: "PedidoCompra",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoCompraProduto_PedidoCompraId",
                table: "PedidoCompraProduto",
                column: "PedidoCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoCompraProduto_ProdutoId",
                table: "PedidoCompraProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoVenda_VendedorId",
                table: "PedidoVenda",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoVendaProduto_PedidoVendaId",
                table: "PedidoVendaProduto",
                column: "PedidoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoVendaProduto_ProdutoId",
                table: "PedidoVendaProduto",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoCompraProduto");

            migrationBuilder.DropTable(
                name: "PedidoVendaProduto");

            migrationBuilder.DropTable(
                name: "PedidoCompra");

            migrationBuilder.DropTable(
                name: "PedidoVenda");

            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Preco",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "TipoEstoque",
                table: "Produto");

            migrationBuilder.AddColumn<double>(
                name: "PesoEstoque",
                table: "Produto",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantidadeEstoque",
                table: "Produto",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
