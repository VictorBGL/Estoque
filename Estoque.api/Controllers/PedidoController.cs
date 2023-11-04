using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Filters;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.api.Controllers
{
    [Route("api/pedido")]
    [Authorize]
    public class PedidoController : MainController
    {
        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public PedidoController(
            IMapper mapper,
            INotifiable notifiable,
            ILogger<PedidoController> logger,
            IPedidoService pedidoService) : base(notifiable)
        {
            _mapper = mapper;
            _pedidoService = pedidoService;
            _logger = logger;
        }

        [HttpPost("venda/filtro")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PedidoVendaResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> Filter([FromBody] PedidoVendaFilterModel model, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            try
            {
                var resultado = _mapper.Map<IEnumerable<PedidoVendaResponseModel>>(await _pedidoService.FiltrarPedidoVenda(model.DataInicio, model.DataFim, model.NomeVendedor, model.DirecaoOrdem, model.ColunaOrdem));

                var resultadoPaginado = PaginacaoListModel<PedidoVendaResponseModel>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

                return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resultadoPaginado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost("venda")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "vendedor")]
        public virtual async Task<IActionResult> Insert([FromBody] PedidoVendaModel model)
        {
            try
            {
                var pedido = _mapper.Map<PedidoVenda>(model);

                await _pedidoService.InserirVenda(pedido);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("venda/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PedidoVendaResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "vendedor")]
        public virtual async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            try
            {
                var resultado = await _pedidoService.BuscarVendaPorId(id);

                return CustomResponse(_mapper.Map<PedidoVendaResponseModel>(resultado));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("venda/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "vendedor")]
        public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PedidoVendaModel model)
        {
            try
            {
                var pedido = _mapper.Map<PedidoVenda>(model);

                await _pedidoService.AtualizarVenda(id, pedido);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("venda/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "vendedor")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _pedidoService.DeleteVendaAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("compra/filtro")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PedidoCompraResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public async Task<IActionResult> FilterCompra([FromBody] PedidoCompraFilterModel model, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            try
            {
                var resultado = _mapper.Map<IEnumerable<PedidoCompraResponseModel>>(await _pedidoService.FiltrarPedidoCompra(model.DataInicio, model.DataFim, model.NomeComprador, model.NomeFornecedor, model.DirecaoOrdem, model.ColunaOrdem));

                var resultadoPaginado = PaginacaoListModel<PedidoCompraResponseModel>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

                return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resultadoPaginado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost("compra")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> InsertCompra([FromBody] PedidoCompraModel model)
        {
            try
            {
                var pedido = _mapper.Map<PedidoCompra>(model);

                await _pedidoService.InserirCompra(pedido);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("compra/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PedidoCompraResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> GetCompraId([FromRoute] Guid id)
        {
            try
            {
                var resultado = await _pedidoService.BuscarCompraPorId(id);

                return CustomResponse(_mapper.Map<PedidoCompraResponseModel>(resultado));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("compra/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> UpdateCompra([FromRoute] Guid id, [FromBody] PedidoCompraModel model)
        {
            try
            {
                var pedido = _mapper.Map<PedidoCompra>(model);

                await _pedidoService.AtualizarCompra(id, pedido);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("compra/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> DeleteCompra([FromRoute] Guid id)
        {
            try
            {
                await _pedidoService.DeleteCompraAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
