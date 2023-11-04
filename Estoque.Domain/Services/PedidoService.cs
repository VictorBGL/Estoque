using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Extensions;
using Estoque.Domain.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace Estoque.Domain.Services
{
    public class PedidoService : Service<PedidoVenda>, IPedidoService
    {
        private readonly IPedidoVendaRepository _pedidoVendaRepository;
        private readonly IPedidoCompraRepository _pedidoCompraRepository;
        private readonly IRepository<Produto> _produtoRepository;
        private readonly IHubContext<NotificacaoHub> _hub;

        public PedidoService(
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IPedidoVendaRepository pedidoVendaRepository,
            IRepository<Produto> produtoRepository,
            IPedidoCompraRepository pedidoCompraRepository,
            IHubContext<NotificacaoHub> hub) : base(pedidoVendaRepository, notifiable, unitOfWork, aspnetUser)
        {
            _pedidoVendaRepository = pedidoVendaRepository;
            _produtoRepository = produtoRepository;
            _pedidoCompraRepository = pedidoCompraRepository;
            _hub = hub;
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

        public async Task InserirVenda(PedidoVenda pedido)
        {
            if(pedido.Produtos == null || pedido.Produtos.Count() == 0)
            {
                _notifiable.AddNotification("Adicione ao menos um produto ao pedido.");
                return;
            }

            foreach(var produto in pedido.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == produto.ProdutoId);

                if(produtoDb.Estoque == null || produtoDb.Estoque < produto.Quantidade)
                {
                    _notifiable.AddNotification($"A quantidade está maior que o estoque do produto {produtoDb.Nome}");
                    return;
                }

                var estoqueAtual = produtoDb.Estoque - produto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);
            }

            await _pedidoVendaRepository.InsertAsync(pedido);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PedidoVenda> BuscarVendaPorId(Guid id)
        {
            var pedido =  await _pedidoVendaRepository.Table
                                            .Include(p => p.Vendedor)
                                            .Include(p => p.Produtos)
                                                .ThenInclude(p => p.Produto)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            return pedido;
        }

        public async Task AtualizarVenda(Guid id, PedidoVenda pedido)
        {
            if (pedido.Produtos == null || pedido.Produtos.Count() == 0)
            {
                _notifiable.AddNotification("Adicione ao menos um produto ao pedido.");
                return;
            }

            var venda = await _pedidoVendaRepository.Table
                                                .Include(p => p.Produtos)
                                                    .ThenInclude(p => p.Produto)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            foreach(var vendaProduto in venda.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == vendaProduto.ProdutoId);
                var estoqueAtual = produtoDb.Estoque + vendaProduto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);
            }

            _unitOfWork.Commit();

            foreach (var produto in pedido.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == produto.ProdutoId);

                if (produtoDb.Estoque == null || produtoDb.Estoque < produto.Quantidade)
                {
                    _notifiable.AddNotification($"A quantidade está maior que o estoque do produto {produtoDb.Nome}");
                    return;
                }

                var estoqueAtual = produtoDb.Estoque - produto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);
            }

            _pedidoVendaRepository.UpdateMany(pedido);
            _unitOfWork.Commit();
        }

        public async Task DeleteVendaAsync(Guid id)
        {
            var venda = await _pedidoVendaRepository.Table
                                                .Include(p => p.Produtos)
                                                    .ThenInclude(p => p.Produto)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            foreach (var vendaProduto in venda.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == vendaProduto.ProdutoId);
                var estoqueAtual = produtoDb.Estoque + vendaProduto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);
            }

            await _pedidoVendaRepository.DeleteAsync(id);
            _unitOfWork.Commit();
        }

        public async Task<IEnumerable<PedidoCompra>> FiltrarPedidoCompra(DateTime? dataInicio, DateTime? dataFim, string? nomeComprador, string? nomeFornecedor, string direcaoOrdem, string colunaOrdem)
        {
            var compras = _pedidoCompraRepository.Table
                                                .Include(p => p.Comprador)
                                                .Include(p => p.Fornecedor)
                                                .Include(p => p.Produtos)
                                                    .ThenInclude(p => p.Produto)
                                                .Where(p => (dataInicio == null || p.Data.Date >= dataInicio) &&
                                                            (dataFim == null || p.Data.Date <= dataFim) &&
                                                            (string.IsNullOrEmpty(nomeComprador) || p.Comprador.Nome.ToUpper().Contains(nomeComprador.ToUpper())) &&
                                                            (string.IsNullOrEmpty(nomeFornecedor) || (p.FornecedorId != null && p.Fornecedor.Nome.ToUpper().Contains(nomeFornecedor.ToUpper()))))
                                                .AsQueryable();


            if (!string.IsNullOrEmpty(colunaOrdem))
                compras = compras.OrderBy(colunaOrdem, direcaoOrdem);
            else
                compras = compras.OrderBy(p => p.Data);


            return await compras.ToListAsync();
        }

        public async Task InserirCompra(PedidoCompra pedido)
        {
            if (pedido.Produtos == null || pedido.Produtos.Count() == 0)
            {
                _notifiable.AddNotification("Adicione ao menos um produto ao pedido.");
                return;
            }

            foreach (var produto in pedido.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == produto.ProdutoId);

                if(produtoDb != null)
                {
                    var estoqueAtual = produtoDb.Estoque + produto.Quantidade;

                    produtoDb.AtualizarEstoque(estoqueAtual.Value);

                    _produtoRepository.Update(produtoDb);
                }
            }

            await _pedidoCompraRepository.InsertAsync(pedido);
            await _unitOfWork.CommitAsync();

            var compraRealizada = await _pedidoCompraRepository.Table
                                                            .Include(p => p.Produtos)
                                                                .ThenInclude(p => p.Produto)
                                                            .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            await _hub.OnNovaCompra($"Uma nova compra foi realizada, o estoque do(s) seguinte(s) produto(s) foi atualizado: {string.Join(", ", compraRealizada.Produtos.Select(p => p.Produto.Nome))}");
        }

        public async Task<PedidoCompra> BuscarCompraPorId(Guid id)
        {
            var pedido = await _pedidoCompraRepository.Table
                                            .Include(p => p.Comprador)
                                            .Include(p => p.Fornecedor)
                                            .Include(p => p.Produtos)
                                                .ThenInclude(p => p.Produto)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            return pedido;
        }

        public async Task AtualizarCompra(Guid id, PedidoCompra pedido)
        {
            if (pedido.Produtos == null || pedido.Produtos.Count() == 0)
            {
                _notifiable.AddNotification("Adicione ao menos um produto ao pedido.");
                return;
            }

            var compra = await _pedidoCompraRepository.Table
                                                .Include(p => p.Produtos)
                                                    .ThenInclude(p => p.Produto)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            foreach (var Produto in compra.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == Produto.ProdutoId);
                var estoqueAtual = produtoDb.Estoque - Produto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);
            }

            _unitOfWork.Commit();

            foreach (var produto in pedido.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == produto.ProdutoId);

                var estoqueAtual = produtoDb.Estoque + produto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);
            }

            _pedidoCompraRepository.UpdateMany(pedido);
            _unitOfWork.Commit();

            var compraRealizada = await _pedidoCompraRepository.Table
                                                            .Include(p => p.Produtos)
                                                                .ThenInclude(p => p.Produto)
                                                            .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            await _hub.OnNovaCompra($"Uma compra foi atualizada, o estoque do(s) seguinte(s) produto(s) foi atualizado: {string.Join(", ", compraRealizada.Produtos.Select(p => p.Produto.Nome))}");
        }

        public async Task DeleteCompraAsync(Guid id)
        {
            var compra = await _pedidoCompraRepository.Table
                                                .Include(p => p.Produtos)
                                                    .ThenInclude(p => p.Produto)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            var produtosAtualizados = new List<Produto>();

            foreach (var compraProduto in compra.Produtos)
            {
                var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == compraProduto.ProdutoId);
                var estoqueAtual = produtoDb.Estoque - compraProduto.Quantidade;

                produtoDb.AtualizarEstoque(estoqueAtual.Value);

                _produtoRepository.Update(produtoDb);

                produtosAtualizados.Add(produtoDb);
            }

            await _pedidoCompraRepository.DeleteAsync(id);
            _unitOfWork.Commit();

            await _hub.OnNovaCompra($"Uma compra foi removida, o estoque do(s) seguinte(s) produto(s) foi atualizado: {string.Join(", ", produtosAtualizados.Select(p => p.Nome))}");
        }
    }
}
