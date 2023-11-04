using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infra.Repositories
{
    public class PedidoVendaRepository : Repository<PedidoVenda>, IPedidoVendaRepository
    {

        private readonly Context _context;

        public PedidoVendaRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void UpdateMany(PedidoVenda venda)
        {
            var dbProdutos = _context.Set<PedidoVendaProduto>()
                                        .Where(p => p.PedidoVendaId == venda.Id)
                                        .AsNoTracking()
                                        .ToList();

            _context.Upsert(venda)
                .ThenUpdateMany(dbProdutos, p => p.Produtos);
        }
    }
}
