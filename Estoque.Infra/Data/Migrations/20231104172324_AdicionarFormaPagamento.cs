using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarFormaPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormaPagamento",
                table: "PedidoVenda",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormaPagamento",
                table: "PedidoVenda");
        }
    }
}
