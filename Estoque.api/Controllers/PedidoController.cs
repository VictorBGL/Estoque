using AutoMapper;
using Estoque.Api.Core.Controllers;
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
        public PedidoController(
            IMapper mapper,
            INotifiable notifiable,
            IPedidoService pedidoService) : base(notifiable)
        {
            _mapper = mapper;
            _pedidoService = pedidoService;
        }

        
    }
}
