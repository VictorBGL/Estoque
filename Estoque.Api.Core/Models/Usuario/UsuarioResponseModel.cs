using Estoque.Core.Entities;

namespace Estoque.Api.Core.Models
{
    public class UsuarioResponseModel
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Acesso { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<PedidoCompraResponseModel> Compras { get; set; }
        public IEnumerable<PedidoVendaResponseModel> Vendas { get; set; }
    }
}
