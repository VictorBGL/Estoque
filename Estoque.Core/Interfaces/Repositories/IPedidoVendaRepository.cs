using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IPedidoVendaRepository : IRepository<PedidoVenda>
    {
        void UpdateMany(PedidoVenda venda);
    }
}
