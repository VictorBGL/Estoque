using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IFornecedorService : IService<Fornecedor>
    {
        Task<IEnumerable<Fornecedor>> Filtrar(bool? ativo, string? termo, string? direcaoOrdem, string? colunaOrdem);
    }
}
