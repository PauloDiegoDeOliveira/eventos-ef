using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Identity.Policies.HorarioComercial
{
    public class HorarioComercialHandler : AuthorizationHandler<HorarioComercialRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HorarioComercialRequirement requirement)
        {
            DateTime horarioAtual = DateTime.Now;
            if (horarioAtual.Hour >= 7 && horarioAtual.Hour <= 23)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}