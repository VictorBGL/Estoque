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


        public void Cadastrar()
        {
            DataCadastro = DateTime.Now;
        }

        public void Atualizar(Fornecedor fornecedor)
        {
            Nome = fornecedor.Nome;
            Email = fornecedor.Email;
            Telefone = fornecedor.Telefone;
            CpfCnpj = fornecedor.CpfCnpj;
            Ativo = fornecedor.Ativo;
        }

        public void Remover()
        {
            Removido = true;
        }
    }
}
