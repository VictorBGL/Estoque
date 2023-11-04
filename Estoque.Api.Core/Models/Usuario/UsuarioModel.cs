using System.ComponentModel.DataAnnotations;

namespace Estoque.Api.Core.Models
{
    public class UsuarioModel
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Acesso { get; set; }
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string SenhaConfirmacao { get; set; }
    }
}
