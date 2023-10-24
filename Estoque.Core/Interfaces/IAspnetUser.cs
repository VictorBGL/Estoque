using System.Security.Claims;

namespace Estoque.Core.Interfaces
{
    public interface IAspnetUser
    {
        string Name { get; }
        string Action { get; }
        string GetUserId();
        string GetEmpresaId();
        string GetUserRole();
        string GetUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
