namespace Estoque.Core.Entities
{
    public class PedidoVenda : EntityBase
    {
        protected PedidoVenda() { }

        public DateTime Data { get; private set; }
        public decimal ValorTotal { get; private set; }
        public string? Observacao { get; private set; }
        public Guid VendedorId { get; private set; }
        public virtual Usuario Vendedor { get; private set; }
        public virtual ICollection<PedidoVendaProduto> Produtos { get; private set; }
    }
}
