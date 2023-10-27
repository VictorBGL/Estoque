using Estoque.Api.Core.Extensions;
using Estoque.Core.Interfaces;
using Estoque.Domain.Notifications;
using Estoque.Domain.Services;
using Estoque.Infra.Data;

namespace Estoque.Api.Configuration
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
        {
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IProdutoService, ProdutoService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<INotifiable, Notifiable>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAspnetUser, AspnetUser>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
