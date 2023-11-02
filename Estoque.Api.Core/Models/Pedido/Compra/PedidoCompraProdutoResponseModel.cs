using Estoque.Core.Entities;

namespace Estoque.Api.Core.Models
{
    public class PedidoCompraProdutoResponseModel
    {
        public Guid Id { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorPorQuantidade { get; set; }
        public decimal Valor { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid PedidoCompraId { get; set; }

        public ProdutoResponseModel Produto { get; set; }
    }
}
