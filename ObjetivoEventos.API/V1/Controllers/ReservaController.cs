using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Hubs;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/reservas")]
    [ApiController]
    public class ReservaController : MainController
    {
        private readonly IReservaApplication reservaApplication;
        private readonly MessageHub messageHub;

        public ReservaController(IReservaApplication reservaApplication,
                                 INotificador notificador,
                                 MessageHub messageHub,
                                 IUser user) : base(notificador, user)
        {
            this.reservaApplication = reservaApplication;
            this.messageHub = messageHub;
        }

        /// <summary>
        /// Retorna todas as reservas com filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersBase"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Reserva, ViewReservaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersBase parametersBase)
        {
            ViewPagedListDto<Reserva, ViewReservaDto> result = await reservaApplication.GetPaginationAsync(parametersBase);

            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhuma reserva foi encontrada.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Reservas encontradas.");
        }

        /// <summary>
        /// Insere um nova reserva.
        /// </summary>
        /// <param name="postReservaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewReservaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostAsync([FromBody] PostReservaDto postReservaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(postReservaDto.ConnectionId))
            {
                NotificarErro("O id da conexão não pode ser nulo.");
                return CustomResponse(ModelState);
            }

            ViewReservaDto inserido = await reservaApplication.PostAsync(postReservaDto);

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir a reserva.");
                return CustomResponse(ModelState);
            }

            await messageHub.SendMessage(inserido);

            return CustomResponse(inserido, "Reserva criada com sucesso!");
        }

        /// <summary>
        /// Altera uma reserva.
        /// </summary>
        /// <param name="putReservaDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewReservaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutAsync([FromBody] PutReservaDto putReservaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewReservaDto atualizado = await reservaApplication.PutAsync(putReservaDto);

            if (atualizado is null)
            {
                NotificarErro("Nenhuma reserva foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            await messageHub.SendMessage(atualizado);

            return CustomResponse(atualizado, "Reserva atualizada com sucesso!");
        }

        /// <summary>
        /// Exclui uma reserva.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir uma reserva o mesmo será removido permanentemente da base.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewReservaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewReservaDto result = await reservaApplication.DeleteAsync(id);

            if (result is null)
                return CustomResponse(ModelState);

            await messageHub.SendMessage(result);

            return CustomResponse(result, "Reserva excluída com sucesso!");
        }

        /// <summary>
        /// Altera o status de uma reserva.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewReservaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusReservaDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewReservaDto result = await reservaApplication.PutStatusAsync(putStatusDto);

            if (result is null)
            {
                NotificarErro("Nenhuma reserva foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            await messageHub.SendMessage(result);

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Reserva atualizada para ativo com sucesso!"),
                Status.Inativo => CustomResponse(result, "Reserva atualizada para inativo com sucesso!"),
                Status.Excluido => CustomResponse(result, "Reserva atualizada para excluído com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }

        /// <summary>
        ///  Altera a situação de uma lista de reservas.
        /// </summary>
        /// <param name="putSituacaoReservaDto"></param>
        /// <returns></returns>
        [HttpPut("situacao")]
        [ProducesResponseType(typeof(List<ViewReservaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutSituacaoReservasAsync([FromForm] PutSituacaoReservaDto putSituacaoReservaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putSituacaoReservaDto.SituacaoReserva == 0)
            {
                NotificarErro("Nenhuma situação reserva selecionada.");
                return CustomResponse(ModelState);
            }

            List<ViewReservaDto> result = await reservaApplication.PutSituacaoReservaAsync(putSituacaoReservaDto);

            if (result is null)
            {
                NotificarErro("Erro ao atualizar a reserva.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Situação da reserva alterado com sucesso!");
        }
    }
}