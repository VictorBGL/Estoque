namespace Estoque.Core.Entities
{
    public class Usuario : EntityBase
    {
        public Usuario(string nome, string telefone, string email, string acesso, double salarioBruto, int dependentes, double pensao, string status)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Acesso = acesso;
            SalarioBruto = salarioBruto;
            Dependentes = dependentes;
            Pensao = pensao;
            Status = status;
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Acesso { get; set; }
        public double SalarioBruto { get; set; }
        public int Dependentes { get; set; }
        public double Pensao { get; set; }
        public string Status { get; set; }

        public void Atualizar(Usuario usuario)
        {
            Nome = usuario.Nome;
            SalarioBruto = usuario.SalarioBruto;
            Telefone = usuario.Telefone;
            Dependentes = usuario.Dependentes;
            Pensao = usuario.Pensao;
            Status = usuario.Status;
            Email = usuario.Email;
        }

        public void Deletar()
        {
            Status = "Inativo";
        }
    }
}
