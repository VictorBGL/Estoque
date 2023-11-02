using Estoque.Core.Entities;

namespace Estoque.Api.Core.Models
{
    public class PedidoCompraResponseModel
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacao { get; set; }
        public IEnumerable<PedidoCompraProdutoResponseModel> Produtos { get; set; }
        public FornecedorResponseModel Fornecedor { get; set; }
        public CompradorResponseModel Comprador { get; set; }
    }

    public class CompradorResponseModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
