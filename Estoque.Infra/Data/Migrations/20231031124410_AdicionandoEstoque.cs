using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Estoque",
                table: "Produto",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estoque",
                table: "Produto");
        }
    }
}
