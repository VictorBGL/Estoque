using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IPedidoCompraRepository : IRepository<PedidoCompra>
    {
        void UpdateMany(PedidoCompra compra);
    }
}
