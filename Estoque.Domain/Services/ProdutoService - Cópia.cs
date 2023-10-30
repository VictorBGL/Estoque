using Estoque.Core.Entities;
using Estoque.Core.Interfaces;

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

        public async Task<Produto> InsertProduto(Produto produto)
        {
            await _produtoRepository.InsertAsync(produto);
            _unitOfWork.Commit();

            return produto;
        }

        public async Task<Produto> GetId(Guid id)
        {
            var produto = await _produtoRepository.GetAsync(id);

            return produto;
        }
    }
}
