namespace Estoque.Api.Core.Models
{
    public class ProdutoResponseModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Removido { get; set; }
        public decimal? QuantidadeEstoque { get; set; }
        public double? PesoEstoque { get; set; }
    }
}
