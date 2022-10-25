using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Setor;
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
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/setores")]
    [ApiController]
    public class SetorController : MainController
    {
        private readonly ISetorApplication applicationSetor;

        public SetorController(ISetorApplication applicationSetor,
                               INotificador notificador,
                               IUser user) : base(notificador, user)
        {
            this.applicationSetor = applicationSetor;
        }

        /// <summary>
        /// Retorna todos os setores com filtro e paginação de dados.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Setor, ViewSetorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersPalavraChave parameters)
        {
            ViewPagedListDto<Setor, ViewSetorDto> result = await applicationSetor.GetPaginationAsync(parameters);

            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhum setor foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Setores encontrados.");
        }

        /// <summary>
        /// Retorna os dados de dentro de um setor baseado em Ids.
        /// </summary>
        /// <param name="postViewDadosSetorDto"></param>
        /// <returns></returns>
        [HttpGet("dados")]
        [ProducesResponseType(typeof(List<ViewSetorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSetorDadosByIds([FromQuery] PostViewDadosSetorDto postViewDadosSetorDto)
        {
            List<ViewSetorDto> result = await applicationSetor.GetSetorDadosByIds(postViewDadosSetorDto);

            if (result is null)
            {
                NotificarErro("Nenhum setor foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Setor encontrado.");
        }

        /// <summary>
        /// Insere um novo setor.
        /// </summary>
        /// <param name="postSetorDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewSetorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostSetorDto postSetorDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewSetorDto inserido = await applicationSetor.PostAsync(postSetorDto);

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir o setor.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(inserido, "Setor criado com sucesso!");
        }

        /// <summary>
        /// Altera um setor.
        /// </summary>
        /// <param name="putSetorDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewSetorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutSetorDto putSetorDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewSetorDto atualizado = await applicationSetor.PutAsync(putSetorDto);
            if (atualizado is null)
            {
                NotificarErro("Nenhum setor foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(atualizado, "Setor atualizado com sucesso!");
        }

        /// <summary>
        /// Inserção automática de cadeiras para o setor baseando nos valores de colunas e linhas.
        /// </summary>
        /// <param name="postAutomaticoSetorDto"></param>
        /// <returns></returns>
        [HttpPost("automatico")]
        [ProducesResponseType(typeof(ViewSetorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostSetorByCadeirasAutomaticoAsync([FromBody] PostAutomaticoSetorDto postAutomaticoSetorDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewSetorDto inserido = await applicationSetor.PostSetorByCadeirasAutomaticoAsync(postAutomaticoSetorDto);

            return CustomResponse(inserido, "Setor criado com sucesso!");
        }

        /// <summary>
        /// Atualização automática de cadeiras em um setor baseado no total de colunas e número de linhas.
        /// </summary>
        /// <param name="putAutomaticoSetorDto"></param>
        /// <returns></returns>
        [HttpPut("automatico")]
        [ProducesResponseType(typeof(ViewSetorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutSetorByCadeirasAutomaticoAsync([FromBody] PutAutomaticoSetorDto putAutomaticoSetorDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewSetorDto atualizado = await applicationSetor.PutSetorByCadeirasAutomaticoAsync(putAutomaticoSetorDto);
            if (atualizado is null)
            {
                NotificarErro("Nenhum setor foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(atualizado, "Cadeiras atualizadas no setor com sucesso!");
        }

        /// <summary>
        /// Exclui um setor.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir uma setor o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewSetorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewSetorDto result = await applicationSetor.PutStatusAsync(new PutStatusDto(id, Status.Excluido));

            if (result is null)
            {
                NotificarErro("Nenhuma setor foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Setor excluído com sucesso!");
        }

        /// <summary>
        /// Altera o status de um setor.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewSetorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusSetorDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewSetorDto result = await applicationSetor.PutStatusAsync(putStatusDto);
            if (result is null)
            {
                NotificarErro("Nenhum setor foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Setor atualizada para ativa com sucesso!"),
                Status.Inativo => CustomResponse(result, "Setor atualizada para inativa com sucesso!"),
                Status.Excluido => CustomResponse(result, "Setor atualizada para excluída com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }
    }
}