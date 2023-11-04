using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Extensions;
using Estoque.Api.Core.Filters;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Estoque.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Estoque.Api.Controllers
{
    [Route("api/usuario")] 
    public class UsuarioController : MainController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public UsuarioController(
            IUsuarioService service,
            IMapper mapper,
            INotifiable notifiable,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<AppSettings> appSettings,
            IPasswordHasher<IdentityUser> passwordHasher) : base(notifiable)
        {
            _appSettings = appSettings.Value;
            _usuarioService = service;
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UsuarioLoginResponseModel), 200)]
        public virtual async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var usuario = await _usuarioService.BuscarUsuarioPorEmail(loginModel.Email);

                if (usuario == null || !usuario.Ativo)
                {
                    return Ok(new
                    {
                        Authenticated = false,
                        Message = "Falha ao autenticar, usuário inativo"
                    });
                }
                
                var identityUser = await _userManager.FindByIdAsync(usuario.Id.ToString());

                if (identityUser == null)
                {
                    return Ok(new
                    {
                        Authenticated = false,
                        Message = "Falha ao autenticar, usuário inativo"
                    });
                }

                var result = await _signInManager.PasswordSignInAsync(identityUser, loginModel.Senha, true, true);

                if (result.Succeeded)
                {
                    var tokenResponse = await GerarJwt(identityUser.UserName);
                    return Ok(tokenResponse);
                }
                else
                {
                    return Ok(new
                    {
                        Authenticated = false,
                        Message = "Falha ao autenticar, credenciais inválidas"
                    });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("filtro")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "financeiro")]
        public async Task<IActionResult> Filter([FromBody] UsuarioFilterModel model, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            try
            {
                var resultado = _mapper.Map<IEnumerable<UsuarioResponseModel>>(await _usuarioService.Filtrar(model.Ativo, model.Termo, model.DirecaoOrdem, model.ColunaOrdem));

                var resultadoPaginado = PaginacaoListModel<UsuarioResponseModel>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

                return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resultadoPaginado);
            }
            catch (Exception ex)
            {
                return InternalServerError($"Erro: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "admin")]
        public async Task<IActionResult> CriarConta([FromBody] UsuarioModel model)
        {
            var usuario = await _usuarioService.BuscarUsuarioPorEmail(model.Email.ToLower());

            if (usuario != null)
            {
                _notifiable.AddNotification("Email já existente");
                return BadRequest(new BadRequestModel(_notifiable.GetNotificationsAsObject));
            }

            if (string.IsNullOrEmpty(model.Nome) || string.IsNullOrEmpty(model.Email))
            {
                _notifiable.AddNotification("Dados do usuário não fornecidos");
                return BadRequest(new BadRequestModel(_notifiable.GetNotificationsAsObject));
            }

            var identityUser = new IdentityUser(model.Email.ToLower());
            identityUser.Email = model.Email.ToLower();
            identityUser.NormalizedEmail = model.Email.ToUpper();

            try
            {
                var result = await _userManager.CreateAsync(identityUser);

                if (result.Succeeded)
                {
                    var novoUsuario = new Usuario(Guid.Parse(identityUser.Id), model.Nome, model.Telefone, model.Email.ToLower(), model.Acesso, true);

                    await _usuarioService.CadastrarUsuario(novoUsuario);

                    await GerarJwt(model.Email);

                    var hash = _passwordHasher.HashPassword(identityUser, model.Senha);
                    identityUser.SecurityStamp = Guid.NewGuid().ToString();
                    identityUser.PasswordHash = hash;

                    await _userManager.UpdateAsync(identityUser);

                    return CustomResponse();
                }

                foreach (var error in result.Errors)
                {
                    _notifiable.AddNotification("Erro no registro", error.Description);
                }

                _notifiable.AddNotification("Erro no registro");
                return BadRequest(new BadRequestModel(_notifiable.GetNotificationsAsObject));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestModel(_notifiable.GetNotificationsAsObject));
            }
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "admin")]
        public async Task<IActionResult> EditarConta([FromRoute] Guid id, [FromBody] UsuarioModel model)
        {
            if (string.IsNullOrEmpty(model.Nome) || string.IsNullOrEmpty(model.Email))
            {
                _notifiable.AddNotification("Dados do usuário não fornecidos");
                return BadRequest(new BadRequestModel(_notifiable.GetNotificationsAsObject));
            }

            try
            {
                var usuario = _mapper.Map<Usuario>(model);

                await _usuarioService.AtualizarUsuario(id, usuario);

                await GerarJwt(model.Email);

                var identityUser = await _userManager.FindByIdAsync(id.ToString());

                if (model.Senha != null)
                {
                    var hash = _passwordHasher.HashPassword(identityUser, model.Senha);
                    identityUser.SecurityStamp = Guid.NewGuid().ToString();
                    identityUser.PasswordHash = hash;

                    await _userManager.UpdateAsync(identityUser);
                }

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestModel(_notifiable.GetNotificationsAsObject));
            }
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [ClaimsAuthorize("acesso", "admin")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _usuarioService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        private async Task<UsuarioLoginResponseModel> GerarJwt(string email)
        {
            var identityUser = await _userManager.FindByNameAsync(email);
            var claims = await _userManager.GetClaimsAsync(identityUser);
            var userRoles = await _userManager.GetRolesAsync(identityUser);

            var usuario = await _usuarioService.BuscarUsuarioPorEmail(email);

            var identityClaims = await ObterClaimsUsuario(userRoles, claims, usuario);
            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, usuario, claims);
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private UsuarioLoginResponseModel ObterRespostaToken(string encodedToken, Usuario usuario, IEnumerable<Claim> claims)
        {
            return new UsuarioLoginResponseModel
            {
                Authenticated = true,
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(24).TotalHours,
                UsuarioToken = new UsuarioToken
                {
                    Id = usuario.Id.ToString(),
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(IList<string>? roles, ICollection<Claim> claims, Usuario? usuario)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, usuario.Nome));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, ToUnixEpochDate(DateTime.UtcNow.AddHours(24)).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in roles)
            {
                claims.Add(new Claim("role", userRole));

                var roleDb = await _roleManager.FindByNameAsync(userRole);
                var roleClaims = await _roleManager.GetClaimsAsync(roleDb);

                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }

            claims.Add(new Claim("acesso", usuario.Acesso));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return await Task.FromResult(identityClaims);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
