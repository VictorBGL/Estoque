using Estoque.Core.Entities;

namespace Estoque.Api.Core.Models
{
    public class PedidoVendaProdutoResponseModel
    {
        public Guid Id { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorPorQuantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorAdicional { get; set; }

        public ProdutoResponseModel Produto { get; set; }
    }
}
