using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Product;

namespace Estoque.Core.Entities
{
    public class PedidoCompraProduto : EntityBase
    {
        public decimal Quantidade { get; private set; }
        public decimal ValorPorQuantidade { get; private set; }
        public decimal Valor { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid PedidoCompraId { get; private set; }

        public virtual Produto Produto { get; set; }
        public virtual PedidoCompra PedidoCompra { get; set; }
    }
}
