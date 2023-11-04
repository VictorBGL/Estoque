using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<IEnumerable<Usuario>> Filtrar(bool? ativo, string? termo, string? direcaoOrdem, string? colunaOrdem);
        Task<Usuario> BuscarUsuarioPorEmail(string email);
        Task CadastrarUsuario(Usuario usuario);
        Task AtualizarUsuario(Guid id, Usuario usuario);
    }
}
