namespace Estoque.Core.Entities
{
    public class PedidoCompra : EntityBase
    {
        protected PedidoCompra() { }

        public DateTime Data { get; private set; }
        public decimal ValorTotal { get; private set; }
        public string? Descricao { get; private set; }
        public Guid? FornecedorId { get; private set; }
        public Guid CompradorId { get; private set; }
        public virtual ICollection<PedidoCompraProduto> Produtos { get; private set; }
        public virtual Fornecedor Fornecedor { get; private set; }
        public virtual Usuario Comprador { get; private set; }
    }
}
