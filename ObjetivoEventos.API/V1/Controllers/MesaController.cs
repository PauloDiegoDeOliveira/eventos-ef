using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/mesas")]
    [ApiController]
    public class MesaController : MainController
    {
        private readonly IMesaApplication applicationMesa;

        public MesaController(IMesaApplication applicationMesa,
                              INotificador notificador,
                              IUser user) : base(notificador, user)
        {
            this.applicationMesa = applicationMesa;
        }

        /// <summary>
        /// Retorna todas as mesas com filtro e paginação de dados.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Mesa, ViewMesaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersPalavraChave parameters)
        {
            ViewPagedListDto<Mesa, ViewMesaDto> result = await applicationMesa.GetPaginationAsync(parameters);
            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhuma mesa foi encontrada.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Mesas encontradas.");
        }

        /// <summary>
        /// Insere uma nova mesa.
        /// </summary>
        /// <param name="postMesaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewMesaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostMesaDto postMesaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewMesaDto inserido = await applicationMesa.PostAsync(postMesaDto);

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir a mesa.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(inserido, "Mesa criada com sucesso!");
        }

        /// <summary>
        /// Altera uma mesa.
        /// </summary>
        /// <param name="putMesaDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewMesaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutMesaDto putMesaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewMesaDto atualizado = await applicationMesa.PutAsync(putMesaDto);
            if (atualizado is null)
            {
                NotificarErro("Nenhua mesa foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(atualizado, "Mesa atualizada com sucesso!");
        }

        /// <summary>
        /// Exclui uma mesa.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir uma mesa o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewMesaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewMesaDto result = await applicationMesa.PutStatusAsync(new PutStatusDto(id, Status.Excluido));

            if (result is null)
            {
                NotificarErro("Nenhuma mesa foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Mesa excluída com sucesso!");
        }

        /// <summary>
        /// Altera o status de uma mesa.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewMesaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusMesaDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewMesaDto result = await applicationMesa.PutStatusAsync(putStatusDto);

            if (result is null)
            {
                NotificarErro("Nenhuma mesa foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Mesa atualizada para ativa com sucesso!"),
                Status.Inativo => CustomResponse(result, "Mesa atualizada para inativa com sucesso!"),
                Status.Excluido => CustomResponse(result, "Mesa atualizada para excluída com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }
    }
}