using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDescricaoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "PedidoVendaProduto");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "PedidoVenda");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "PedidoCompraProduto");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "PedidoCompra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Descricao",
                table: "PedidoVendaProduto",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "PedidoVenda",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descricao",
                table: "PedidoCompraProduto",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "PedidoCompra",
                type: "longtext",
                nullable: true);
        }
    }
}
