using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infra.Repositories
{
    public class PedidoCompraRepository : Repository<PedidoCompra>, IPedidoCompraRepository
    {

        private readonly Context _context;

        public PedidoCompraRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void UpdateMany(PedidoCompra compra)
        {
            var dbProdutos = _context.Set<PedidoCompraProduto>()
                                        .Where(p => p.PedidoCompraId == compra.Id)
                                        .AsNoTracking()
                                        .ToList();

            _context.Upsert(compra)
                .ThenUpdateMany(dbProdutos, p => p.Produtos);
        }
    }
}
