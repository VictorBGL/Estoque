using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IPedidoService : IService<PedidoVenda>
    {
        Task<IEnumerable<PedidoVenda>> FiltrarPedidoVenda(DateTime? dataInicio, DateTime? dataFim, string? nomeVendedor, string direcaoOrdem, string colunaOrdem);
        Task InserirVenda(PedidoVenda pedido);
        Task<PedidoVenda> BuscarVendaPorId(Guid id);
        Task<IEnumerable<PedidoCompra>> FiltrarPedidoCompra(DateTime? dataInicio, DateTime? dataFim, string? nomeComprador, string? nomeFornecedor, string direcaoOrdem, string colunaOrdem);
        Task InserirCompra(PedidoCompra pedido);
        Task<PedidoCompra> BuscarCompraPorId(Guid id);
    }
}
