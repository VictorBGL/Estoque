namespace Estoque.Api.Core.Models
{
    public class PedidoCompraModel
    {
        public Guid? Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacao { get; set; }
        public Guid? FornecedorId { get; set; }
        public Guid CompradorId { get; set; }
        public IEnumerable<PedidoCompraProdutoModel> Produtos { get; set; }
    }
}
