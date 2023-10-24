using Estoque.Core.Entities;
using Estoque.Core.Interfaces;

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

      
    }
}
