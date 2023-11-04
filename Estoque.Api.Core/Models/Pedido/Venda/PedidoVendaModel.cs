namespace Estoque.Api.Core.Models
{
    public class PedidoVendaModel
    {
        public Guid? Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacao { get; set; }
        public string FormaPagamento { get; set; }
        public Guid VendedorId { get; set; }
        public IEnumerable<PedidoVendaProdutoModel> Produtos { get; set; }
    }
}
