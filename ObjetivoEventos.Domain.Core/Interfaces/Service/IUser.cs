using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ObjetivoEventos.Domain.Core.Interfaces.Service
{
    public interface IUser
    {
        string Name { get; }

        bool IsAuthenticated();

        bool IsInRole(string role);

        Guid GetUserId();

        string GetUserEmail();

        string GetUserRole();

        IEnumerable<string> GetUserClaims();

        IEnumerable<Claim> GetClaimsIdentity();
    }
}