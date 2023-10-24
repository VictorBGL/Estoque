﻿using Estoque.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Estoque.Api.Core.Extensions
{
    public class AspnetUser : IAspnetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspnetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext?.User?.GetUserName();

        public string Action => _accessor.HttpContext.Request.Method;

        public string GetUserId()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUsuarioId() : "";

        }

        public string GetEmpresaId()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetEmpresaId() : "";
        }

        public string GetUserRole()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserRole() : "";
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.InRole(role);
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsuarioId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetEmpresaId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("empresaId");
            return claim?.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("name");
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }

        public static bool InRole(this ClaimsPrincipal principal, string role)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Role);

            return claim?.Value.Equals(role) == true;
        }
    }
}
