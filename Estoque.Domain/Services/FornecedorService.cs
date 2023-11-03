using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Domain.Services
{
    public class FornecedorService : Service<Fornecedor>, IFornecedorService
    {
        private readonly IRepository<Fornecedor> _fornecedorRepository;

        public FornecedorService(
            IRepository<Fornecedor> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _fornecedorRepository = repository;
        }

        public async Task<IEnumerable<Fornecedor>> Filtrar(bool? ativo, string? termo, string? direcaoOrdem, string? colunaOrdem)
        {
            var fornecedor = _fornecedorRepository.Table
                                                .Where(p => !p.Removido && (ativo == null || p.Ativo == ativo))
                                                .AsQueryable();

            if (!string.IsNullOrEmpty(termo))
                fornecedor = fornecedor.Where(p => p.Nome.ToUpper().Contains(termo.ToUpper()));

            if (!string.IsNullOrEmpty(colunaOrdem))
            {
                if (colunaOrdem.ToUpper() == "STATUS")
                    fornecedor = fornecedor.OrderByProp(p => p.Ativo ? "A" : "I", direcaoOrdem);

                else
                    fornecedor = fornecedor.OrderBy(colunaOrdem, direcaoOrdem);
            }
            else
                fornecedor = fornecedor.OrderBy(p => p.Nome);


            return await fornecedor.ToListAsync();
        }
    }
}
