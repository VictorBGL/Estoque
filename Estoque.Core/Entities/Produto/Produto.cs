namespace Estoque.Core.Entities
{
    public class Produto : EntityBase
    {
        protected Produto() 
        {

        }

        public Produto(string nome, string descricao, bool ativo, decimal preco, string tipoEstoque, decimal? estoque, string? imagem)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Preco = preco;
            TipoEstoque = tipoEstoque;
            Estoque = estoque;
            Imagem = imagem;
            DataCriacao = DateTime.Now;
        }

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }
        public string TipoEstoque { get; private set; }
        public decimal? Estoque { get; private set; }
        public string? Imagem { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public virtual ICollection<PedidoVendaProduto> PedidosVenda { get; private set; }
        public virtual ICollection<PedidoCompraProduto> PedidosCompra { get; private set; }

        public void Atualizar(Produto entity)
        {
            Nome = entity.Nome;
            Descricao = entity.Descricao;
            Ativo = entity.Ativo;
            Preco = entity.Preco;
            TipoEstoque = entity.TipoEstoque;
            Estoque = entity.Estoque;
            Imagem = entity.Imagem;
            DataAtualizacao = DateTime.Now;
        }

        public void AtualizarEstoque(decimal estoque)
        {
            Estoque = estoque;
            DataAtualizacao = DateTime.Now;
        }

        public void Remover()
        {
            Removido = true;
        }
    }
}
