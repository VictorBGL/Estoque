using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IProdutoService : IService<Produto>
    {
        Task<Produto> InsertProduto(Produto produto);
        Task<Produto> GetId(Guid id);
    }
}
