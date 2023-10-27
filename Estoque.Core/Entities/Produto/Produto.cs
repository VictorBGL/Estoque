namespace Estoque.Core.Entities
{
    public class Produto : EntityBase
    {
        protected Produto() 
        {

        }

        public Produto(string nome, string descricao, bool ativo)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
        }

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }
        public decimal? QuantidadeEstoque { get; private set; }
        public double? PesoEstoque { get; private set; }

        public void Atualizar(Produto entity)
        {
            Nome = entity.Nome;
            Descricao = entity.Descricao;
            Ativo = entity.Ativo;
        }

        public void Remover()
        {
            Removido = false;
        }
    }
}
