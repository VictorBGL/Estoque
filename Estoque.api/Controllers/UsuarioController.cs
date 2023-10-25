using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Api.Controllers
{
    [Route("api/usuario")] 
    public class UsuarioController : MainController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public UsuarioController(
            IUsuarioService service,
            IMapper mapper,
            INotifiable notifiable,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService,
            ILogger logger) : base(notifiable)
        {
            _usuarioService = service;
            _authService = authService;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> Insert([FromBody] UsuarioModel model)
        {
            try
            {
                var identityUserId = await _authService.CadastrarIdentityUserAsync(model.Email, model.Nome, "Ativo");

                if (identityUserId != Guid.Empty)
                {
                    //Cadastrar Usuario com seu perfil se tiver
                    //await _usuarioService.InsertAsync(identityUserId, _mapper.Map<Usuario>(model), model.LojaId);

                    if (!_notifiable.HasNotification)
                    {
                        if (model.Ativo)
                        {
                            //var encodedToken = _authService.GerarTokenCriacaoSenha(model.Nome, identityUserId);
                            //await _sendGridService.EnviarEmailCriacaoSenhaAsync(model.Email, model.Nome, encodedToken, identityUserId.ToString());
                        }

                        return CustomResponse();
                    }

                    await _authService.DeletarIdentityUserAsync(identityUserId);
                }

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
