using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Local;
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
    [Route("/v{version:apiVersion}/locais")]
    [ApiController]
    public class LocalController : MainController
    {
        private readonly ILocalApplication localApplication;

        public LocalController(ILocalApplication localApplication,
                                 INotificador notificador,
                                 IUser user) : base(notificador, user)
        {
            this.localApplication = localApplication;
        }

        /// <summary>
        /// Retorna todos os locais com filtro e paginação de dados.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Local, ViewLocalDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersPalavraChave parameters)
        {
            ViewPagedListDto<Local, ViewLocalDto> result = await localApplication.GetPaginationAsync(parameters);

            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhum local foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Locais encontrados.");
        }

        /// <summary>
        /// Insere um local.
        /// </summary>
        /// <param name="postLocalDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewLocalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostLocalDto postLocalDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            ViewLocalDto inserido = await localApplication.PostAsync(postLocalDto);

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir o local.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(inserido, "Local criado com sucesso!");
        }

        /// <summary>
        /// Altera um local.
        /// </summary>
        /// <param name="putLocalDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewLocalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutLocalDto putLocalDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewLocalDto atualizado = await localApplication.PutAsync(putLocalDto);

            if (atualizado is null)
            {
                NotificarErro("Nenhum local foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(atualizado, "Local atualizado com sucesso!");
        }

        /// <summary>
        /// Exclui um local.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir um local o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewLocalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewLocalDto result = await localApplication.PutStatusAsync(new PutStatusDto(id, Status.Excluido));

            if (result is null)
            {
                NotificarErro("Nenhum local foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Local excluído com sucesso!");
        }

        /// <summary>
        /// Altera o status de um local.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewLocalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusLocalDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewLocalDto result = await localApplication.PutStatusAsync(putStatusDto);

            if (result is null)
            {
                NotificarErro("Nenhum local foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Local atualizado para ativo com sucesso!"),
                Status.Inativo => CustomResponse(result, "Local atualizado para inativo com sucesso!"),
                Status.Excluido => CustomResponse(result, "Local atualizado para excluído com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }
    }
}