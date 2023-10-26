using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<Usuario> BuscarUsuarioPorEmail(string email);
    }
}
