namespace Estoque.Api.Core.Models
{
    public class PedidoCompraFilterModel : FiltroModelBase
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? NomeComprador { get; set; }
        public string? NomeFornecedor { get; set; }
    }
}
