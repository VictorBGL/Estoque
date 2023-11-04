using Estoque.Core.DTOs;
using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> BuscarDadosDashboard(Guid? vendedorId, DateTime dataInicio, DateTime dataFim);
    }
}
