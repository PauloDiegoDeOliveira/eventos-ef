using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface IPedidoApplication : IApplicationBase<Pedido, ViewPedidoDto, PostPedidoDto, PutPedidoDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Pedido, ViewPedidoDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<ViewPedidoUsuarioAutenticadoDto>> GetByUsuarioAutenticadoAsync();

        Task<List<Pedido>> GetPedidosAtivosSituacaoPedidoAsync(SituacaoPedido situacaoPedido);

        Task<List<ViewPedidoDto>> GetPedidosVencidosPorDiasAsync(int dias);

        Task<List<ViewPedidoDto>> VerificaPedidosVencidosPeloEventoAsync(bool pedidosVencidos, List<Pedido> pedidos);

        Task<ViewReservaDto> PutValidateQrCodeAsync(PutValidaQrCodePedidoDto putValidateQrCodePedidoDto);

        Task<ViewPedidoDto> PutTrocaPedidoAsync(PutPedidoTrocaDto putPedidoTrocaDto);

        Task<ViewPedidoDto> PutSituacaoPedidoAsync(PutSituacaoPedidoDto putSituacaoPedidoDto);

        Task<List<ViewPedidoDto>> PutPedidosAtivosParaInativosByEventosExpirados(List<ViewEventoDto> eventos);

        Task CriarEmail(string subject, string template, string userName, string numeroPedido, List<string> usersEmail);

        bool ValidarId(Guid id);

        bool ValidarInputReservaEventoPedidos(PostPedidoDto pedido);

        bool ValidarReservaPedidos(PostPedidoDto pedido);
    }
}