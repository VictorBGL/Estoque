namespace Estoque.Core.Entities
{
    public class Fornecedor : EntityBase
    {
        protected Fornecedor() { }

        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public string CpfCnpj { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }

        public virtual ICollection<PedidoCompra> Pedidos { get; private set; }
    }
}
