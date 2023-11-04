namespace Estoque.Api.Core.Models
{
    public class UsuarioFilterModel : FiltroModelBase
    {
        public string? Termo { get; set; }
        public bool? Ativo { get; set; }
    }
}
