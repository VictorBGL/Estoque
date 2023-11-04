using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Filters;
using Estoque.Api.Core.Models;
using Estoque.Core.DTOs;
using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.api.Controllers
{
    [Route("api/dashboard")]
    [Authorize]
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
        [ProducesResponseType(typeof(DashboardDTO), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public async Task<IActionResult> BuscarDadosDashboard([FromBody] DashboardFilterModel model)
        {
            try
            {
                var resultado = await _dashboardService.BuscarDadosDashboard(model.VendedorId, model.DataInicio, model.DataFim);

                return CustomResponse(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }
    }
}
