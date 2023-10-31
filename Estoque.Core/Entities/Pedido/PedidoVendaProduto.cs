namespace Estoque.Core.Entities
{
    public class PedidoVendaProduto : EntityBase
    {
        public decimal Quantidade { get; private set; }
        public decimal ValorPorQuantidade { get; private set; }
        public decimal Valor { get; private set; }
        public decimal Descricao { get; private set; }
        public decimal ValorDesconto { get; private set; }
        public decimal ValorAdicional { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid PedidoVendaId { get; private set; }

        public virtual Produto Produto { get; set; }
        public virtual PedidoVenda PedidoVenda { get; set; }
    }
}
