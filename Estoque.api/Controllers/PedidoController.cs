using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Models;
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
        [ProducesResponseType(typeof(IEnumerable<PedidoVendaResponseModel>), 200)]
        [Produces("application/json")]
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

        //[HttpPost()]
        //[ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        //[Produces("application/json")]
        //public virtual async Task<IActionResult> Insert([FromBody] ProdutoModel model)
        //{
        //    try
        //    {
        //        var produto = _mapper.Map<Produto>(model);

        //        var resultado = _produtoService.Inserir(produto);

        //        return CustomResponse(_mapper.Map<ProdutoResponseModel>(resultado.Result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        [HttpGet("venda/{id}")]
        [ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            try
            {
                var resultado = await _pedidoService.GetAsync(id);

                return CustomResponse(_mapper.Map<PedidoVendaResponseModel>(resultado));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        //[Produces("application/json")]
        //public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProdutoModel model)
        //{
        //    try
        //    {
        //        var produto = _mapper.Map<Produto>(model);

        //        var resultado = _produtoService.Atualizar(id, produto);

        //        return CustomResponse(_mapper.Map<ProdutoResponseModel>(resultado.Result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        [HttpDelete("venda/{id}")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _pedidoService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
