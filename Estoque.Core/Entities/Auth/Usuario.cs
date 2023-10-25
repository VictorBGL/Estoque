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

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Acesso { get; set; }
        public bool Ativo { get; set; }

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
