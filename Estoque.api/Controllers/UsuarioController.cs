using AutoMapper;
using Estoque.Api.Core.Controllers;
using Estoque.Api.Core.Models;
using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Estoque.Api.Controllers
{
    [Route("api/usuario")] 
    public class UsuarioController : MainController
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        public UsuarioController(
            IUsuarioService service,
            IMapper mapper,
            INotifiable notifiable,
            //RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IPasswordHasher<IdentityUser> passwordHasher) : base(notifiable)
        {
            _usuarioService = service;
            //_roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UsuarioLoginResponseModel), 200)]
        [Produces("application/json")]
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

                //await GerarSenha(identityUser, loginModel.Senha);

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
                    var guid = Guid.Parse(identityUser.Id);

                    var tokenResponse = await GerarJwt(identityUser.Email);
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

        private async Task<string> GerarSenha(IdentityUser usuario, string senha)
        {
            var hash = _passwordHasher.HashPassword(usuario, senha);
            usuario.SecurityStamp = Guid.NewGuid().ToString();
            usuario.PasswordHash = hash;
            await _userManager.UpdateAsync(usuario);

            return await Task.FromResult(hash);
        }

        private async Task<UsuarioLoginResponseModel> GerarJwt(string email)
        {
            var identityUser = await _userManager.FindByNameAsync(email);
            var claims = await _userManager.GetClaimsAsync(identityUser);
            var userRoles = await _userManager.GetRolesAsync(identityUser);

            var usuario = await _usuarioService.BuscarUsuarioPorEmail(email);

            var identityClaims = await ObterClaimsUsuario(userRoles, claims, identityUser, usuario);
            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, usuario, claims);
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            byte[] keyBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes("MecSecretTd2Familia");
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "Victor",
                Audience = "https://localhost",
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
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
                }
            };
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(IList<string>? roles, ICollection<Claim> claims, IdentityUser user, Usuario? usuario)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, usuario.Nome));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, ToUnixEpochDate(DateTime.UtcNow.AddHours(24)).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in roles)
            {
                claims.Add(new Claim("role", userRole));

                //var roleDb = await _roleManager.FindByNameAsync(userRole);
                //var roleClaims = await _roleManager.GetClaimsAsync(roleDb);

                //foreach (var roleClaim in roleClaims)
                //{
                //    claims.Add(roleClaim);
                //}
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return await Task.FromResult(identityClaims);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
