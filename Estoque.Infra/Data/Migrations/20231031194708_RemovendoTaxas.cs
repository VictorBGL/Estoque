using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovendoTaxas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valorAdicional",
                table: "PedidoVendaProduto");

            migrationBuilder.DropColumn(
                name: "valorDesconto",
                table: "PedidoVendaProduto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "valorAdicional",
                table: "PedidoVendaProduto",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valorDesconto",
                table: "PedidoVendaProduto",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
