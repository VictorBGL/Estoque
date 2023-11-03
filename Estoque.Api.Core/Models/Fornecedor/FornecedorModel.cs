namespace Estoque.Api.Core.Models
{
    public class FornecedorModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CpfCnpj { get; set; }
        public bool Ativo { get; set; }
    }
}
