using Estoque.Core.DTOs;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<DashboardDTO> BuscarDadosDashboard(Guid? vendedorId, DateTime dataInicio, DateTime dataFim)
        {
            decimal quantidadeProdutosVendidos = 0;
            decimal faturamento = 0;
            decimal quantidadeProdutosComprados = 0;
            decimal valorGasto = 0;

            var vendas = await _vendaRepository.Table
                                                 .Include(p => p.Produtos)
                                                 .Where(p => (p.Data.Date >= dataInicio.Date &&
                                                              p.Data.Date <= dataFim.Date) && 
                                                              (vendedorId == null || p.VendedorId == vendedorId))
                                                 .ToListAsync();

            foreach (var venda in vendas)
            {
                var quantidadeProdutos = venda.Produtos.Sum(x => x.Quantidade);

                quantidadeProdutosVendidos += quantidadeProdutos;

                faturamento += venda.ValorTotal;
            }

            var compras = await _compraRepository.Table
                                                 .Include(p => p.Produtos)
                                                 .Where(p => (p.Data.Date >= dataInicio.Date &&
                                                              p.Data.Date <= dataFim.Date))
                                                 .ToListAsync();

            foreach (var compra in compras)
            {
                var quantidadeComprados = compra.Produtos.Sum(x => x.Quantidade);

                quantidadeProdutosComprados += quantidadeComprados;

                valorGasto += compra.ValorTotal;
            }

            var dash = new DashboardDTO(quantidadeProdutosVendidos, vendas.Count(), faturamento, compras.Count(), quantidadeProdutosComprados, valorGasto);

            return dash;
        }
    }
}
