using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Models;
using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.api.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : MainController
    {
        private readonly ILogger _logger;
        private readonly IDashboardService _dashboardService;

        public DashboardController(INotifiable notifiable, ILogger<DashboardController> logger, IDashboardService dashboardService) : base(notifiable)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpPost("filtro")]
        public async Task<IActionResult> BuscarDadosDashboard([FromBody] DashboardFilterModel model)
        {
            try
            {
                //var resultado = await _dashboardService.BuscarDadosDashboard(model.DataInicio, model.DataFim);
                return CustomResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }
    }
}
