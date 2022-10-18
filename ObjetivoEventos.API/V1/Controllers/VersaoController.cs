using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ObjetivoEventos.Domain.Enums;

namespace ObjetivoEventos.Application.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/versao")]
    [ApiController]
    public class VersaoController : ControllerBase
    {
        private readonly Ambiente ambiente;
        const string versao = "Esta é a versão V1.";

        public VersaoController(IWebHostEnvironment environment)
        {
            ambiente = environment.IsProduction() ? Ambiente.Producao :
              environment.IsStaging() ? Ambiente.Homologacao : Ambiente.Desenvolvimento;
        }

        /// <summary>
        /// Informa a versão da API.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public string Valor()
        {
            return versao;
        }

        /// <summary>
        /// Informa o ambiente da API.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ambiente")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public string AmbienteAtual()
        {
            return ambiente.ToString();
        }
    }
}