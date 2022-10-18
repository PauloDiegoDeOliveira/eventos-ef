using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Core.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjetivoEventos.Application.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador notificador;
        public readonly IUser AppUser;

        protected bool UsuarioAutenticado { get; set; }
        protected string UsuarioNome { get; set; }
        protected string UsuarioEmail { get; set; }
        protected Guid UsuarioId { get; set; }
        protected IEnumerable<string> UsuarioClaims { get; set; }

        protected MainController(INotificador notificador,
                                 IUser appUser)
        {
            this.notificador = notificador;
            AppUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UsuarioAutenticado = true;
                UsuarioNome = appUser.Name;
                UsuarioEmail = appUser.GetUserEmail();
                UsuarioId = appUser.GetUserId();
                UsuarioClaims = appUser.GetUserClaims();
            }
        }

        protected bool OperacaoValida()
        {
            return !notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(object result = null, string mensagem = "")
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    mensagem,
                    sucesso = true,
                    dados = result
                });
            }

            return BadRequest(new
            {
                sucesso = false,
                erros = notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                NotificarErroModelInvalida(modelState);

            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;

                if (errorMsg.ToString().Contains("Error converting value") || errorMsg.ToString().Contains("The value "))
                    NotificarErro("Os dados inseridos estão em um formato inválido.");
                else
                    NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            notificador.Handle(new Notificacao(mensagem));
        }

        protected void NotificarErro(List<string> mensagemList)
        {
            foreach (string erro in mensagemList)
            {
                NotificarErro(erro);
            }
        }
    }
}