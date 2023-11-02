namespace Estoque.Api.Core.Models
{
    public class ProdutoFilterModel : FiltroModelBase
    {
        public string? Termo { get; set; }
        public bool? Ativo { get; set; }
    }
}
