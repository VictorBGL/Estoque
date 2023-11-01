namespace Estoque.Api.Core.Models
{
    public class PedidoVendaModel
    {
        public DateTime Data { get; set; } 
        public decimal ValorTotal { get; set; }
        public string? Descricao { get; set; }
        public IEnumerable<PedidoVendaProdutoModel> Produtos { get; set; }
    }
}
