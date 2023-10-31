using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IProdutoService : IService<Produto>
    {
        Task<IEnumerable<Produto>> Filtrar(string? termo, string? direcaoOrdem, string? colunaOrdem);
        Task<Produto> Inserir(Produto produto);
        Task<Produto> Atualizar(Guid id, Produto produto);
        Task<Produto> BuscarPorId(Guid id);
    }
}
