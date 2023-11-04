using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Filters;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.api.Controllers
{
    [Route("api/fornecedor")]
    [Authorize]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public FornecedorController(
            IFornecedorService service,
            IMapper mapper,
            INotifiable notifiable,
            ILogger<FornecedorController> logger) : base(notifiable)
        {
            _fornecedorService = service;
            _mapper = mapper;
        }

        [HttpPost("filtro")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FornecedorResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public async Task<IActionResult> Filter([FromBody] FornecedorFilterModel model, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            try
            {
                var resultado = _mapper.Map<IEnumerable<FornecedorResponseModel>>(await _fornecedorService.Filtrar(model.Ativo, model.Termo, model.DirecaoOrdem, model.ColunaOrdem));

                var resultadoPaginado = PaginacaoListModel<FornecedorResponseModel>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

                return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resultadoPaginado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> Insert([FromBody] FornecedorModel model)
        {
            try
            {
                var fornecedor = _mapper.Map<Fornecedor>(model);
                fornecedor.Cadastrar();

                var resultado = await _fornecedorService.InsertAsync(fornecedor);

                return CustomResponse(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FornecedorResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            try
            {
                var resultado = await _fornecedorService.GetAsync(id);

                return CustomResponse(_mapper.Map<FornecedorResponseModel>(resultado));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] FornecedorModel model)
        {
            try
            {
                var produto = _mapper.Map<Fornecedor>(model);

                await _fornecedorService.UpdateAsync(id, produto);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _fornecedorService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
