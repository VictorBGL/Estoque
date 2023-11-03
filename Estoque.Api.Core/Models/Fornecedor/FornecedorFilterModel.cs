namespace Estoque.Api.Core.Models
{
    public class FornecedorFilterModel : FiltroModelBase
    {
        public string? Termo { get; set; }
        public bool? Ativo{ get; set; }
    }
}
