using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Domain.Services
{
    public class UsuarioService : Service<Usuario>, IUsuarioService
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public UsuarioService(
            IRepository<Usuario> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _usuarioRepository = repository;
        }

        public async Task<IEnumerable<Usuario>> Filtrar(bool? ativo, string? termo, string? direcaoOrdem, string? colunaOrdem)
        {
            var usuarios = _usuarioRepository.Table
                                                .Include(p => p.Compras)
                                                    .ThenInclude(p => p.Produtos)
                                                        .ThenInclude(p => p.Produto)
                                                .Include(p => p.Compras)
                                                    .ThenInclude(p => p.Fornecedor)
                                                .Include(p => p.Vendas)
                                                    .ThenInclude(p => p.Produtos)
                                                        .ThenInclude(p => p.Produto)
                                                .Where(p => ativo == null || p.Ativo == ativo)
                                                .AsQueryable();

            if (!string.IsNullOrEmpty(termo))
                usuarios = usuarios.Where(p => p.Nome.ToUpper().Contains(termo.ToUpper()));

            if (!string.IsNullOrEmpty(colunaOrdem))
                usuarios = usuarios.OrderBy(colunaOrdem, direcaoOrdem);
            else
                usuarios = usuarios.OrderBy(p => p.Nome);

            return await usuarios.ToListAsync();
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {
            var usuario =  await _usuarioRepository.Table.FirstOrDefaultAsync(p => p.Email == email);

            return usuario;
        }

        public async Task CadastrarUsuario(Usuario usuario)
        {
            await _usuarioRepository.InsertAsync(usuario);
            _unitOfWork.Commit();  
        }

        public async Task AtualizarUsuario(Guid id, Usuario usuario)
        {
            var entity = await _usuarioRepository.Table.FirstOrDefaultAsync(p => p.Id == id);

            entity.Atualizar(usuario);

            _usuarioRepository.Update(entity);
            _unitOfWork.Commit();
        }
    }
}
