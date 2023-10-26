using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
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

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {
          var usuario =  await _usuarioRepository.Table.FirstOrDefaultAsync(p => p.Email == email);

            return usuario;
        }
    }
}
