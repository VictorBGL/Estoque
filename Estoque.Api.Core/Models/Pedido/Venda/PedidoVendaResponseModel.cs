namespace Estoque.Api.Core.Models
{
    public class PedidoVendaResponseModel
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacao { get; set; }
        public string FormaPagamento { get; set; }
        public VendedorResponseModel Vendedor { get; set; }
        public IEnumerable<PedidoVendaProdutoResponseModel> Produtos { get; set; }
    }

    public class VendedorResponseModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
