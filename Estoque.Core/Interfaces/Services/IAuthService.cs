using Microsoft.Graph.Models;
using System.Security.Claims;

namespace Estoque.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LogarMicrosoftAuthorizationCodeAsync(string authorizationCode, string state, string redirectUri); //Authorization Code Flow
        Task<User?> LogarMicrosoftOpenIdAsync(string accessToken, string state); //OpenId Flow
        string GerarTokenCriacaoSenha(string nomeUsuario, Guid usuarioId);
        Task<bool> ValidarTokenCriacaoSenhaAsync(string token, string usuarioId);
        Task<Guid> CadastrarIdentityUserAsync(string email, string nome, string status);
        Task DeletarIdentityUserAsync(Guid id);
        Task<List<Claim?>> CriarClaims(Guid permissaoId, IEnumerable<Guid> acoesId);
        Task<List<Claim>?> BuscarRoleClaimsAsync(Guid perfilId);
        Task AtualizarPermissoesIdentityUserAsync(Guid usuarioId, List<Claim> claimsUsuario, List<Claim> claimsPerfil);
        Task AtualizarUserRoleAsync(Guid usuarioId, Guid perfilId);
        Task<bool> CadastrarIdentityRoleAsync(Guid perfilId, string nomePerfil, string tipoUsuario);
        Task AtualizarRoleAsync(Guid perfilId, string nome, string tipoUsuario);
        Task AtualizarIdentityUserAsync(Guid id, string nome, string email);
        Task RemoverUsersRoleAsync(Guid perfilId);
        Task AtualizarPermissoesIdentityRoleAsync(Guid perfilId, List<Claim> claimsPerfil);
        Task DeletarIdentityRoleAsync(Guid perfilId);
    }
}
