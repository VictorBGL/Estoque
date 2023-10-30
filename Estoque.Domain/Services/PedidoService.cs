using Estoque.Core.Entities;
using Estoque.Core.Interfaces;

namespace Estoque.Domain.Services
{
    public class PedidoService : Service<PedidoVenda>, IPedidoService
    {
        private readonly IRepository<PedidoVenda> _pedidoVendaRepository;

        public PedidoService(
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IRepository<PedidoVenda> pedidoVendaRepository) : base(pedidoVendaRepository, notifiable, unitOfWork, aspnetUser)
        {
            _pedidoVendaRepository = pedidoVendaRepository;
        }


    }
}
