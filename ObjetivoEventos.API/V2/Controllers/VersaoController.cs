using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ObjetivoEventos.Application.V2.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    public class VersaoController : ControllerBase
    {
        private const string versao = "Esta é a versão V2.";

        /// <summary>
        /// Informa a versão da API.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/v{version:apiVersion}/versao")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public string Valor()
        {
            return versao;
        }
    }
}