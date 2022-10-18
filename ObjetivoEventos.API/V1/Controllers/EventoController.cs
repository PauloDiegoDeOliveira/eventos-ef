using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Extensions;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Application.Utilities.Caminhos;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using SerilogTimings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/evento")]
    [ApiController]
    public class EventoController : MainController
    {
        private readonly IEventoApplication eventoApplication;
        private readonly Ambiente ambiente;
        private readonly ILogger<EventoController> logger;

        public EventoController(IEventoApplication eventoApplication,
                                IWebHostEnvironment environment,
                                ILogger<EventoController> logger,
                                INotificador notificador,
                                IUser user) : base(notificador, user)
        {
            this.eventoApplication = eventoApplication;

            ambiente = environment.IsProduction() ? Ambiente.Producao :
              environment.IsStaging() ? Ambiente.Homologacao : Ambiente.Desenvolvimento;

            this.logger = logger;
        }

        /// <summary>
        ///  Retorna todos os eventos com filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersEvento"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Evento, ViewEventoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersEvento parametersEvento)
        {
            ViewPagedListDto<Evento, ViewEventoDto> result = await eventoApplication.GetPaginationAsync(parametersEvento);

            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhum evento foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Eventos encontrados.");
        }

        /// <summary>
        /// Retorna os detalhes de um evento.
        /// </summary>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        [HttpGet("detalhes")]
        [ProducesResponseType(typeof(ViewEventoDetalhesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventoDetalhes([FromQuery] Guid eventoId)
        {
            ViewEventoDetalhesDto result = await eventoApplication.GetDetalhesAsync(eventoId);

            if (result is null)
            {
                NotificarErro("Nenhum evento foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Detalhes do evento.");
        }

        /// <summary>
        /// Retorna a disponibilidade de um evento.
        /// </summary>
        /// <param name="eventoId"></param>
        /// <param name="setorId"></param>
        /// <returns></returns>
        [HttpGet("disponibilidade")]
        [ProducesResponseType(typeof(ViewEventoDisponibilidadeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDisponibilidadeEvento([FromQuery] Guid eventoId, Guid setorId)
        {
            ViewEventoDisponibilidadeDto result = await eventoApplication.GetDisponibilidadeAsync(eventoId, setorId);

            if (result is null)
            {
                NotificarErro("Nenhum evento foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Disponibilidades do evento e setor.");
        }

        /// <summary>
        /// Insere um novo evento.
        /// </summary>
        /// <param name="postEventoDto"></param>
        /// <param name="diretorios"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewEventoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromForm] PostEventoDto postEventoDto, Diretorios diretorios)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            logger.LogInformation("Objeto recebido {@postEventoDto}", postEventoDto);

            if (!await PathSystem.ValidateURLs(diretorios.ToString(), ambiente))
            {
                NotificarErro("Diretório não encontrado.");
                return CustomResponse(ModelState);
            }

            Dictionary<string, string> Urls = await PathSystem.GetURLs(diretorios.ToString(), ambiente);

            ViewEventoDto inserido;
            using (Operation.Time("----------------Tempo de adição de um novo evento----------------"))
            {
                logger.LogInformation("----------------Foi requisitada a inserção de um novo evento----------------");

                inserido = await eventoApplication.PostAsync(postEventoDto, Urls["IP"], Urls["DNS"], Urls["SPLIT"]); ;
            }

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir o evento.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(inserido, "Evento criado com sucesso!");
        }

        /// <summary>
        /// Altera um evento.
        /// </summary>
        /// <param name="putEventoDto"></param>
        /// <param name="diretorios"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewEventoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromForm] PutEventoDto putEventoDto, Diretorios diretorios)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putEventoDto.ImagemUpload is not null)
            {
                if (!await PathSystem.ValidateURLs(diretorios.ToString(), ambiente))
                {
                    NotificarErro("Diretório não encontrado.");
                    return CustomResponse(ModelState);
                }

                Dictionary<string, string> Urls = await PathSystem.GetURLs(diretorios.ToString(), ambiente);

                ViewEventoDto atualizado = await eventoApplication.PutAsync(putEventoDto, Urls["IP"], Urls["DNS"], Urls["SPLIT"]);

                if (atualizado is null)
                {
                    NotificarErro("Nenhum evento foi encontrado com o id informado.");
                    return CustomResponse(ModelState);
                }

                return CustomResponse(atualizado, "Evento atualizado com sucesso!");
            }
            else
            {
                ViewEventoDto atualizado = await eventoApplication.PutAsync(putEventoDto, "", "", "");

                if (atualizado is null)
                {
                    NotificarErro("Nenhum evento foi encontrado com o id informado.");
                    return CustomResponse(ModelState);
                }

                return CustomResponse(atualizado, "Evento atualizado com sucesso!");
            }
        }

        /// <summary>
        /// Exclui um evento.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir um evento o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewEventoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewEventoDto result = await eventoApplication.PutStatusAsync(new PutStatusDto(id, Status.Excluido));

            if (result is null)
            {
                NotificarErro("Nenhum evento foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} " + "Usuário: " + User.GetUserEmail(), result);

            return CustomResponse(result, "Evento excluído com sucesso!");
        }

        /// <summary>
        /// Altera o status de um evento.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewEventoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusEventoDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewEventoDto result = await eventoApplication.PutStatusAsync(putStatusDto);
            if (result is null)
            {
                NotificarErro("Nenhum evento foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Evento atualizado para ativo com sucesso!"),
                Status.Inativo => CustomResponse(result, "Evento atualizado para inativo com sucesso!"),
                Status.Excluido => CustomResponse(result, "Evento atualizado para excluído com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }
    }
}