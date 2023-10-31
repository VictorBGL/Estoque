using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IPedidoService : IService<PedidoVenda>
    {
        Task<IEnumerable<PedidoVenda>> FiltrarPedidoVenda(DateTime? dataInicio, DateTime? dataFim, string? nomeVendedor, string direcaoOrdem, string colunaOrdem);
    }
}
