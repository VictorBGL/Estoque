using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaObsPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "PedidoVenda",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "PedidoCompra",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "PedidoVenda");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "PedidoCompra");
        }
    }
}
