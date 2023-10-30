namespace Estoque.Api.Core.Models
{
    public class ProdutoModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public bool Removido { get; set; }
        public string TipoEstoque { get; set; }
        public decimal? Estoque { get; set; }
        public string? Imagem { get; set; }
    }
}
