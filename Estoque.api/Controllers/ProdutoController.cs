using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.api.Controllers
{
    [Route("api/produto")]
    [Authorize]
    public class ProdutoController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public ProdutoController(
            IProdutoService service,
            IMapper mapper,
            INotifiable notifiable,
            ILogger<ProdutoController> logger) : base(notifiable)
        {
            _produtoService = service;
            _mapper = mapper;
        }

        [HttpPost("filtro")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoResponseModel>), 200)]
        [Produces("application/json")]
        public async Task<IActionResult> Filter([FromBody] ProdutoFilterModel model, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            try
            {
                var resultado = _mapper.Map<IEnumerable<ProdutoResponseModel>>(await _produtoService.Filtrar(model.Ativo, model.Termo, model.DirecaoOrdem, model.ColunaOrdem));

                var resultadoPaginado = PaginacaoListModel<ProdutoResponseModel>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

                return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resultadoPaginado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Insert([FromBody] ProdutoModel model)
        {
            try
            {
                var produto = _mapper.Map<Produto>(model);

                var resultado = _produtoService.Inserir(produto);

                return CustomResponse(_mapper.Map<ProdutoResponseModel>(resultado.Result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            try
            {
                var resultado = _produtoService.BuscarPorId(id);

                return CustomResponse(_mapper.Map<ProdutoResponseModel>(resultado.Result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProdutoModel model)
        {
            try
            {
                var produto = _mapper.Map<Produto>(model);

                var resultado = _produtoService.Atualizar(id, produto);

                return CustomResponse(_mapper.Map<ProdutoResponseModel>(resultado.Result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _produtoService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
