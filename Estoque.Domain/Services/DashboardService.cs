using Estoque.Core.Entities;
using Estoque.Core.Interfaces;

namespace Estoque.Domain.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<PedidoVenda> _vendaRepository;
        private readonly IRepository<PedidoCompra> _compraRepository;

        public DashboardService(IRepository<PedidoVenda> vendaRepository, IRepository<PedidoCompra> compraRepository)
        {
            _vendaRepository = vendaRepository;
            _compraRepository = compraRepository;
        }
    }
}
