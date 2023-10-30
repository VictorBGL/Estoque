namespace Estoque.Core.Entities
{
    public class Usuario : EntityBase
    {
        public Usuario(string nome, string telefone, string email, string acesso, bool ativo)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Acesso = acesso;
            Ativo = ativo;
        }

        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string Acesso { get; private set; }
        public bool Ativo { get; private set; }

        public virtual ICollection<PedidoCompra> Compras { get; private set; }
        public virtual ICollection<PedidoVenda> Vendas { get; private set; }

        public void Atualizar(Usuario usuario)
        {
            Nome = usuario.Nome;
            Telefone = usuario.Telefone;
            Ativo = usuario.Ativo;
            Email = usuario.Email;
        }

        public void Deletar()
        {
            Ativo = false;
        }
    }
}
