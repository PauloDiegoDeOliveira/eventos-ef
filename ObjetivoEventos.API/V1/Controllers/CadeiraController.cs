using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Pagination;
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
    [Route("/v{version:apiVersion}/cadeiras")]
    [ApiController]
    public class CadeiraController : MainController
    {
        private readonly ICadeiraApplication applicationCadeira;

        public CadeiraController(ICadeiraApplication applicationCadeira,
                                 INotificador notificador,
                                 IUser user) : base(notificador, user)
        {
            this.applicationCadeira = applicationCadeira;
        }

        /// <summary>
        /// Retorna todas as cadeiras com filtro e paginação de dados.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Cadeira, ViewCadeiraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersPalavraChave parameters)
        {
            ViewPagedListDto<Cadeira, ViewCadeiraDto> result = await applicationCadeira.GetPaginationAsync(parameters);

            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhuma cadeira foi encontrada.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Cadeiras encontradas.");
        }

        /// <summary>
        /// Insere uma nova cadeira.
        /// </summary>
        /// <param name="postCadeiraDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewCadeiraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostCadeiraDto postCadeiraDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewCadeiraDto inserido = await applicationCadeira.PostAsync(postCadeiraDto);

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir a cadeira.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(inserido, "Cadeira criada com sucesso!");
        }

        /// <summary>
        /// Inserção automatica de cadeiras baseando nos valores de colunas e linhas.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="fileira"></param>
        /// <param name="coluna"></param>
        /// <returns></returns>
        [HttpPost("automatico")]
        [ProducesResponseType(typeof(List<ViewCadeiraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAutomaticoAsync([FromForm] string nome, Fileira fileira, int coluna)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (coluna > 1000)
            {
                NotificarErro("O número máximo de coluna deve ser 1000.");
                return CustomResponse(ModelState);
            }

            List<PostCadeiraDto> postCadeiraDtos = new();
            for (int i = 1; i <= coluna; i++)
                postCadeiraDtos.Add(new PostCadeiraDto(nome, fileira, i, Status.Ativo));

            List<ViewCadeiraDto> inserido = await applicationCadeira.PostAutomaticoAsync(postCadeiraDtos);

            return CustomResponse(inserido, "Cadeira criada com sucesso!");
        }

        /// <summary>
        /// Altera uma cadeira.
        /// </summary>
        /// <param name="putCadeiraDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewCadeiraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutCadeiraDto putCadeiraDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewCadeiraDto atualizado = await applicationCadeira.PutAsync(putCadeiraDto);
            if (atualizado is null)
            {
                NotificarErro("Nenhua cadeira foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(atualizado, "Cadeira atualizada com sucesso!");
        }

        /// <summary>
        /// Exclui uma cadeira.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir uma cadeira o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewCadeiraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewCadeiraDto result = await applicationCadeira.PutStatusAsync(new PutStatusDto(id, Status.Excluido));

            if (result is null)
            {
                NotificarErro("Nenhuma cadeira foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Cadeira excluída com sucesso!");
        }

        /// <summary>
        /// Altera o status de uma cadeira.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewCadeiraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusCadeiraDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewCadeiraDto result = await applicationCadeira.PutStatusAsync(putStatusDto);
            if (result is null)
            {
                NotificarErro("Nenhuma cadeira foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Cadeira atualizada para ativa com sucesso!"),
                Status.Inativo => CustomResponse(result, "Cadeira atualizada para inativa com sucesso!"),
                Status.Excluido => CustomResponse(result, "Cadeira atualizada para excluída com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }
    }
}