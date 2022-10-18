using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Dtos.Reserva;
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
    [Route("/v{version:apiVersion}/pedidos")]
    [ApiController]
    public class PedidoController : MainController
    {
        private readonly IPedidoApplication pedidoApplication;

        public PedidoController(IPedidoApplication pedidoApplication,
                                INotificador notificador,
                                IUser user) : base(notificador, user)
        {
            this.pedidoApplication = pedidoApplication;
        }

        /// <summary>
        /// Retorna todos os pedidos com filtro e paginação de dados.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Pedido, ViewPedidoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersPalavraChave parameters)
        {
            ViewPagedListDto<Pedido, ViewPedidoDto> result = await pedidoApplication.GetPaginationAsync(parameters);

            if (result.Pagina.Count is 0)
            {
                NotificarErro("Nenhum pedido foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Pedidos encontrados.");
        }

        /// <summary>
        /// Insere um pedido.
        /// </summary>
        /// <param name="postPedidoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewPedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostAsync([FromBody] PostPedidoDto postPedidoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewPedidoDto inserido = await pedidoApplication.PostAsync(postPedidoDto);

            if (inserido is null)
            {
                NotificarErro("Erro ao inserir o pedido.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(inserido, "Pedido criado com sucesso!");
        }

        /// <summary>
        /// Retorna a lista de pedidos de um usuário autenticado com informações e QR Code de acesso.
        /// </summary>
        /// <returns></returns>
        [HttpGet("autenticado-qrcode")]
        [ProducesResponseType(typeof(List<ViewPedidoUsuarioAutenticadoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByUsuarioAutenticadoAsync()
        {
            List<ViewPedidoUsuarioAutenticadoDto> result = await pedidoApplication.GetByUsuarioAutenticadoAsync();

            if (result is null)
                return CustomResponse(ModelState);

            return CustomResponse(result, "Pedido encontrado com sucesso!");
        }

        /// <summary>
        /// Valida um QR Code de reserva do pedido.
        /// </summary>
        /// <param name="putValidateQrCodePedidoDto"></param>
        /// <returns></returns>
        [HttpPut("validarqrcode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutValidateQrCode([FromForm] PutValidaQrCodePedidoDto putValidateQrCodePedidoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewReservaDto pedido = await pedidoApplication.PutValidateQrCodeAsync(putValidateQrCodePedidoDto);

            if (pedido is null)
                return CustomResponse(ModelState);

            return CustomResponse(null, "QR Code validado com sucesso!");
        }

        /// <summary>
        /// Altera um pedido.
        /// </summary>
        /// <param name="putPedidoDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewPedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutAsync([FromBody] PutPedidoDto putPedidoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewPedidoDto atualizado = await pedidoApplication.PutAsync(putPedidoDto);

            if (atualizado is null)
            {
                NotificarErro("Nenhum pedido foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(atualizado, "Pedido atualizado com sucesso!");
        }

        /// <summary>
        /// Exclui um pedido.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir um pedido o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewPedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewPedidoDto result = await pedidoApplication.PutStatusAsync(new PutStatusDto(id, Status.Excluido));

            if (result is null)
            {
                NotificarErro("Nenhum pedido foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Pedido excluído com sucesso!");
        }

        /// <summary>
        /// Altera o status de um pedido.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewPedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusPedidoDto putStatusDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putStatusDto.Status == 0)
            {
                NotificarErro("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            ViewPedidoDto result = await pedidoApplication.PutStatusAsync(putStatusDto);

            if (result is null)
            {
                NotificarErro("Nenhum pedido foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            return result.Status switch
            {
                Status.Ativo => CustomResponse(result, "Pedido atualizado para ativo com sucesso!"),
                Status.Inativo => CustomResponse(result, "Pedido atualizado para inativo com sucesso!"),
                Status.Excluido => CustomResponse(result, "Pedido atualizado para excluído com sucesso!"),
                _ => CustomResponse(result, "Status atualizado com sucesso!"),
            };
        }

        /// <summary>
        ///  Realiza a troca de uma reserva.
        /// </summary>
        /// <param name="putPedidoTrocaDto"></param>
        /// <returns></returns>
        [HttpPut("troca-reserva")]
        [ProducesResponseType(typeof(ViewPedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutTrocaPedidoAsync([FromForm] PutPedidoTrocaDto putPedidoTrocaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            ViewPedidoDto result = await pedidoApplication.PutTrocaPedidoAsync(putPedidoTrocaDto);

            if (result is null)
            {
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Troca de reserva realizada com sucesso!");
        }

        /// <summary>
        ///  Altera a situação de um pedido.
        /// </summary>
        /// <param name="putSituacaoPedidoDto"></param>
        /// <returns></returns>
        [HttpPut("situacao")]
        [ProducesResponseType(typeof(ViewPedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutSituacaoPedidoAsync([FromForm] PutSituacaoPedidoDto putSituacaoPedidoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (putSituacaoPedidoDto.SituacaoPedido == 0)
            {
                NotificarErro("Nenhuma situação pedido selecionada.");
                return CustomResponse(ModelState);
            }

            ViewPedidoDto result = await pedidoApplication.PutSituacaoPedidoAsync(putSituacaoPedidoDto);

            if (result is null)
            {
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Situação do pedido alterado com sucesso!");
        }
    }
}