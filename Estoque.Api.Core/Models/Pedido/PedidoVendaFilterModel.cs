namespace Estoque.Api.Core.Models
{
    public class PedidoVendaFilterModel : FiltroModelBase
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? NomeVendedor { get; set; }
    }
}
