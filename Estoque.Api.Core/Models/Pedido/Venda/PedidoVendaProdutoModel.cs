namespace Estoque.Api.Core.Models
{
    public class PedidoVendaProdutoModel
    {
        public Guid ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorPorQuantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorAdicional { get; set; }
    }
}
