using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<PedidoVenda>> FiltrarPedidoVenda(DateTime? dataInicio, DateTime? dataFim, string? nomeVendedor, string direcaoOrdem, string colunaOrdem)
        {
            var vendas = _pedidoVendaRepository.Table
                                                .Include(p => p.Vendedor)
                                                .Include(p => p.Produtos)
                                                    .ThenInclude(p => p.Produto)
                                                .Where(p => (dataInicio == null || p.Data.Date >= dataInicio) && 
                                                            (dataFim == null || p.Data.Date <= dataFim) && 
                                                            (string.IsNullOrEmpty(nomeVendedor) || p.Vendedor.Nome.ToUpper().Contains(nomeVendedor.ToUpper())))
                                                .AsQueryable();


            if (!string.IsNullOrEmpty(colunaOrdem))
                vendas = vendas.OrderBy(colunaOrdem, direcaoOrdem);
            else
                vendas = vendas.OrderBy(p => p.Data);


            return await vendas.ToListAsync();
        }
    }
}
