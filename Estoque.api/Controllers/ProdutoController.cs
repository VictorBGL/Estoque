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
        public ProdutoController(
            IProdutoService service,
            IMapper mapper,
            INotifiable notifiable) : base(notifiable)
        {
            _produtoService = service;
            _mapper = mapper;
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Insert([FromBody] ProdutoModel model)
        {
            try
            {
                var produto = _mapper.Map<Produto>(model);

                var resultado = _produtoService.InsertProduto(produto);

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
                var resultado = _produtoService.GetId(id);

                return CustomResponse(_mapper.Map<ProdutoResponseModel>(resultado.Result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
