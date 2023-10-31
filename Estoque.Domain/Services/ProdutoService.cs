using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Domain.Services
{
    public class ProdutoService : Service<Produto>, IProdutoService
    {
        private readonly IRepository<Produto> _produtoRepository;

        public ProdutoService(
            IRepository<Produto> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _produtoRepository = repository;
        }

        public async Task<IEnumerable<Produto>> Filtrar(string? termo, string? direcaoOrdem, string? colunaOrdem)
        {
            var produtos = _produtoRepository.Table
                                                .Where(p => !p.Removido)
                                                .AsQueryable();

            if (!string.IsNullOrEmpty(termo))
                produtos = produtos.Where(p => p.Nome.ToUpper().Contains(termo.ToUpper()));

            if (!string.IsNullOrEmpty(colunaOrdem))
            {
                if (colunaOrdem.ToUpper() == "STATUS")
                    produtos = produtos.OrderByProp(p => p.Ativo ? "A" : "I", direcaoOrdem);

                else if (colunaOrdem.ToUpper() == "PRECO")
                    produtos = produtos.OrderByProp(p => p.Preco.ToString(), direcaoOrdem);

                else if (colunaOrdem.ToUpper() == "DATACRIACAO")
                    produtos = produtos.OrderByProp(p => p.DataCriacao, direcaoOrdem);

                else if (colunaOrdem.ToUpper() == "ESTOQUE")
                    produtos = produtos.OrderByProp(p => p.Estoque == null ? "z" : p.Estoque.ToString(), direcaoOrdem);

                else
                    produtos = produtos.OrderBy(colunaOrdem, direcaoOrdem);
            }
            else
                produtos = produtos.OrderBy(p => p.Nome);


            return await produtos.ToListAsync();
        }

        public async Task<Produto> Inserir(Produto produto)
        {
            produto.SetDataCriacao();
            await _produtoRepository.InsertAsync(produto);
            _unitOfWork.Commit();

            return produto;
        }

        public async Task<Produto> BuscarPorId(Guid id)
        {
            var produto = await _produtoRepository.GetAsync(id);

            return produto;
        }

        public async Task<Produto> Atualizar(Guid id, Produto produto)
        {
            var produtoDb = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == id);

            produtoDb.Atualizar(produto);

            _produtoRepository.Update(produtoDb);
            _unitOfWork.Commit();

            return produtoDb;
        }
    }
}
