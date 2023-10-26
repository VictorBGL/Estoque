using System.ComponentModel.DataAnnotations;

namespace Estoque.Api.Core.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
    public class CriarSenhaModel
    {
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string SenhaConfirmacao { get; set; }
    }

    public class AlterarSenhaModel
    {
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string NovaSenhaConfirmacao { get; set; }
    }

    public class EsqueciMinhaSenhaModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetarSenhaModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string SenhaConfirmacao { get; set; }
        public string Token { get; set; }
    }
}

