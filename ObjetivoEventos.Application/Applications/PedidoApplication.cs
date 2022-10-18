using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Application.Dtos.SMTP;
using ObjetivoEventos.Application.Hubs;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Application.Utilities.CodigoQR;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications
{
    public class PedidoApplication : ApplicationBase<Pedido, ViewPedidoDto, PostPedidoDto, PutPedidoDto, PutStatusDto>, IPedidoApplication
    {
        private readonly IPedidoService pedidoService;
        private readonly IReservaApplication reservaApplication;
        private readonly ISetorApplication setorApplication;
        private readonly IMesaApplication mesaApplication;
        private readonly ICadeiraApplication cadeiraApplication;
        private readonly IEmailApplication emailApplication;
        private readonly IUsuarioApplication usuarioApplication;
        private readonly MessageHub messageHub;
        private readonly IUser user;

        public PedidoApplication(IPedidoService pedidoService, IEmailApplication emailApplication,
                                 IReservaApplication reservaApplication,
                                 IUsuarioApplication usuarioApplication,
                                 ISetorApplication setorApplication,
                                 IMesaApplication mesaApplication,
                                 ICadeiraApplication cadeiraApplication,
                                 MessageHub messageHub,
                                 INotificador notificador,
                                 IUser user,
                                 IMapper mapper) : base(pedidoService, notificador, mapper)
        {
            this.pedidoService = pedidoService;
            this.emailApplication = emailApplication;
            this.reservaApplication = reservaApplication;
            this.usuarioApplication = usuarioApplication;
            this.cadeiraApplication = cadeiraApplication;
            this.mesaApplication = mesaApplication;
            this.setorApplication = setorApplication;
            this.messageHub = messageHub;
            this.user = user;
        }

        public async Task<ViewPagedListDto<Pedido, ViewPedidoDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            PagedList<Pedido> pagedList = await pedidoService.GetPaginationAsync(parametersPalavraChave);
            return new ViewPagedListDto<Pedido, ViewPedidoDto>(pagedList, mapper.Map<List<ViewPedidoDto>>(pagedList));
        }

        public async Task<List<Pedido>> GetPedidosAtivosSituacaoPedidoAsync(SituacaoPedido situacaoPedido)
        {
            return await pedidoService.GetPedidosAtivosSituacaoPedidoAsync(situacaoPedido);
        }

        public async Task<List<ViewPedidoDto>> GetPedidosVencidosPorDiasAsync(int dias)
        {
            return mapper.Map<List<ViewPedidoDto>>(await pedidoService.GetPedidosVencidosPorDiasAsync(dias));
        }

        public async Task<List<ViewPedidoDto>> VerificaPedidosVencidosPeloEventoAsync(bool pedidosVencidos, List<Pedido> pedidos)
        {
            return mapper.Map<List<ViewPedidoDto>>(await pedidoService.VerificaPedidosVencidosPeloEventoAsync(pedidosVencidos, pedidos));
        }

        public async Task<List<ViewPedidoUsuarioAutenticadoDto>> GetByUsuarioAutenticadoAsync()
        {
            List<Pedido> pedidos = await pedidoService.GetByUsuarioAutenticadoAsync();

            if (pedidos == null)
            {
                Notificar("Nenhum pedido foi encontrado.");
                return null;
            }

            List<ViewPedidoUsuarioAutenticadoDto> viewPedidoUsuarioAutenticadoDtos = new();
            foreach (Pedido pedido in pedidos)
            {
                if (pedido.UsuarioId != user.GetUserId())
                {
                    Notificar($"O usuário autenticado não é o dono do pedido de número {pedido.Numero}.");
                }

                if (pedido.Reservas == null && pedido.Reservas.Count <= 0)
                {
                    Notificar($"O pedido de número {pedido.Numero} não possui reservas.");
                }
                else
                {
                    List<ViewReservaQrDto> viewReservaQrDtos = new();
                    foreach (Reserva reserva in pedido.Reservas)
                    {
                        ViewSetorUsuarioAutenticadoDto setor = mapper.Map<ViewSetorUsuarioAutenticadoDto>(await setorApplication.GetByIdAsync(reserva.SetorId));
                        ViewCadeiraDto cadeira = await cadeiraApplication.GetByIdAsync(reserva.CadeiraId);

                        ViewMesaUsuarioAutenticadoDto mesa = new();
                        if (reserva.MesaId != null)
                        {
                            mesa = mapper.Map<ViewMesaUsuarioAutenticadoDto>(await mesaApplication.GetByIdAsync((Guid)reserva.MesaId));
                        }

                        //TODO: Adicionar o caminhho da pagina no QRCode.
                        string CodigoQR = "";
                        if (reserva.SituacaoReserva == SituacaoReserva.CompraFinalizada.ToString() || reserva.SituacaoReserva == SituacaoReserva.Utilizada.ToString())
                        {
                            CodigoQR = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(QRSystem.GenerateByteArray($"?UsuarioId={user.GetUserId()}&PedidoId={pedido.Id}&ReservaId={reserva.Id}")));
                        }

                        viewReservaQrDtos.Add(new ViewReservaQrDto(reserva, setor, mesa, cadeira, CodigoQR));
                    }

                    viewPedidoUsuarioAutenticadoDtos.Add(new ViewPedidoUsuarioAutenticadoDto(pedido, mapper.Map<ViewEventoUsuarioAutenticadoDto>(pedido.Reservas[0].Evento), viewReservaQrDtos));
                }
            }

            return viewPedidoUsuarioAutenticadoDtos;
        }

        public override async Task<ViewPedidoDto> PostAsync(PostPedidoDto post)
        {
            List<Guid> ids = new();
            foreach (ReferenciaReservaDto reservaDto in post.Reservas)
                ids.Add(reservaDto.Id);

            ViewPedidoDto pedido = await base.PostAsync(post);

            if (pedido != null)
            {
                List<ViewReservaDto> viewReservaDtos = await reservaApplication.PutSituacaoReservaAsync(new PutSituacaoReservaDto(ids, SituacaoReserva.AguardandoPagamento));

                if (viewReservaDtos != null)
                {
                    await CriarEmail("Olá {{UserName}}, o seu pedido foi realizado (Objetivo Eventos).", "NewOrder", user.Name, pedido.Numero.ToString(), new List<string> { user.GetUserEmail() });
                }
                else
                {
                    ViewPedidoDto pedidoDeletado = await DeleteAsync(pedido.Id);

                    if (pedidoDeletado == null)
                        return null;
                }
            }

            return pedido;
        }

        public override async Task<ViewPedidoDto> PutAsync(PutPedidoDto put)
        {
            Pedido pedidoConsultado = await pedidoService.GetDetalhesByIdAsync(put.Id);

            if (pedidoConsultado == null)
                return null;

            Guid[] reservaIds = put.Reservas.Select(x => x.Id).ToArray();
            List<Reserva> reservasRemover = pedidoConsultado.Reservas.Where(reserva => !reservaIds.Contains(reserva.Id)).ToList();

            ViewPedidoDto pedidoRetorno = await base.PutAsync(put);

            foreach (Reserva reserva in reservasRemover)
            {
                ViewReservaDto reservaview = mapper.Map<ViewReservaDto>(reserva);
                reservaview.SituacaoReserva = SituacaoReserva.Cancelada;
                await messageHub.SendMessage(reservaview);
            }

            //TODO: Se o front precisar mandar mensagem de reservas atualizadas

            return pedidoRetorno;
        }

        public async Task<ViewReservaDto> PutValidateQrCodeAsync(PutValidaQrCodePedidoDto putValidateQrCodePedidoDto)
        {
            Pedido pedido = await pedidoService.GetDetalhesByIdAsync(putValidateQrCodePedidoDto.PedidoId);

            if (pedido == null)
            {
                Notificar("Nenhum pedido foi encontrado com o id informado.");
                return null;
            }

            if (pedido.UsuarioId != putValidateQrCodePedidoDto.UsuarioId)
            {
                Notificar("O usuário não é o dono do pedido.");
                return null;
            }

            if (pedido.SituacaoPedido != SituacaoPedido.PagamentoAprovado.ToString())
            {
                Notificar("O pagamento do pedido ainda não foi finalizado.");
                return null;
            }

            if (pedido.Reservas == null && pedido.Reservas.Count <= 0)
            {
                Notificar("O pedido não possui reservas.");
                return null;
            }

            Reserva reserva = pedido.Reservas.FirstOrDefault(x => x.Id == putValidateQrCodePedidoDto.ReservaId);

            if (reserva == null)
            {
                Notificar("Reserva não foi encontrada no pedido.");
                return null;
            }

            if (reserva.UsuarioId != putValidateQrCodePedidoDto.UsuarioId)
            {
                Notificar("O usuário não é o dono da reserva.");
                return null;
            }

            if (reserva.SituacaoReserva == SituacaoReserva.Utilizada.ToString())
            {
                Notificar("O QR Code já foi utilizado.");
                return null;
            }

            if (reserva.SituacaoReserva != SituacaoReserva.CompraFinalizada.ToString())
            {
                Notificar("O pagamento da reserva ainda não foi finalizado.");
                return null;
            }

            if (reserva.Evento == null)
            {
                Notificar("Evento não foi encontrado na reserva.");
                return null;
            }

            if (reserva.Evento.DataEvento.Date > DateTime.Today)
            {
                Notificar("Ainda não é a data do evento.");
                return null;
            }

            if (DateTime.Today > reserva.Evento.DataEvento && DateTime.Now > reserva.Evento.DataEvento.AddHours(reserva.Evento.Duracao))
            {
                Notificar("O evento desta reserva já aconteceu.");
                return null;
            }

            List<ViewReservaDto> reservaAtualizada = await reservaApplication.PutSituacaoReservaAsync(new PutSituacaoReservaDto(new List<Guid> { putValidateQrCodePedidoDto.ReservaId }, SituacaoReserva.Utilizada));

            return reservaAtualizada[0];
        }

        public async Task<List<ViewPedidoDto>> PutPedidosAtivosParaInativosByEventosExpirados(List<ViewEventoDto> eventos)
        {
            return mapper.Map<List<ViewPedidoDto>>(await pedidoService.PutPedidosAtivosParaInativosByEventosExpirados(mapper.Map<List<Evento>>(eventos)));
        }

        public async Task<ViewPedidoDto> PutTrocaPedidoAsync(PutPedidoTrocaDto putPedidoTrocaDto)
        {
            Pedido pedido = await pedidoService.GetDetalhesByIdAsync(putPedidoTrocaDto.PedidoId);

            if (pedido == null)
            {
                Notificar("Nenhum pedido foi encontrado com o id informado.");
                return null;
            }

            if (pedido.UsuarioId != user.GetUserId())
            {
                Notificar("O usuário não é o dono do pedido.");
                return null;
            }

            if (pedido.SituacaoPedido != SituacaoPedido.PagamentoAprovado.ToString())
            {
                Notificar("O pagamento do pedido ainda não foi finalizado.");
                return null;
            }

            if (pedido.Reservas == null && pedido.Reservas.Count <= 0)
            {
                Notificar("O pedido não possui reservas.");
                return null;
            }

            Reserva reserva = pedido.Reservas.FirstOrDefault(x => x.Id == putPedidoTrocaDto.ReservaAntiga);

            if (reserva == null)
            {
                Notificar("Reserva não foi encontrada no pedido.");
                return null;
            }

            if (reserva.UsuarioId != user.GetUserId())
            {
                Notificar("O usuário não é o dono da reserva.");
                return null;
            }

            if (reserva.SituacaoReserva == SituacaoReserva.Utilizada.ToString())
            {
                Notificar("A reserva já foi utilizada.");
                return null;
            }

            if (reserva.SituacaoReserva != SituacaoReserva.CompraFinalizada.ToString())
            {
                Notificar("O pagamento da reserva ainda não foi finalizado.");
                return null;
            }

            if (reserva.Evento == null)
            {
                Notificar("Evento não foi encontrado na reserva.");
                return null;
            }

            if (reserva.Evento.DataEvento.Date > DateTime.Today)
            {
                Notificar("Ainda não é a data do evento.");
                return null;
            }

            if (DateTime.Today > reserva.Evento.DataEvento && DateTime.Now > reserva.Evento.DataEvento.AddHours(reserva.Evento.Duracao))
            {
                Notificar("O evento desta reserva já aconteceu.");
                return null;
            }

            Reserva novaReserva = await reservaApplication.GetReservasDetalhesById(putPedidoTrocaDto.NovaReserva);

            if (novaReserva.Evento == null)
            {
                Notificar("Evento não foi encontrado na nova reserva.");
                return null;
            }

            if (novaReserva.Evento.Id != reserva.Evento.Id)
            {
                Notificar("Não é possivel trocar reservas de eventos diferentes.");
                return null;
            }

            if (novaReserva.SetorId != reserva.SetorId)
            {
                Notificar("Não é possivel trocar reservas de setores diferentes.");
                return null;
            }

            if (novaReserva.SituacaoReserva != SituacaoReserva.Reservado.ToString())
            {
                Notificar("A situação da reserva deve ser Reservado.");
                return null;
            }

            ViewPedidoDto pedidoRetorno = mapper.Map<ViewPedidoDto>(await pedidoService.PutTrocaPedidoReserva(pedido.Id, reserva.Id, novaReserva.Id));

            if (pedidoRetorno == null)
            {
                Notificar("Erro ao realizar a troca.");
                return null;
            }

            ViewReservaDto reservaView = mapper.Map<ViewReservaDto>(reserva);
            reservaView.SituacaoReserva = SituacaoReserva.Cancelada;
            await messageHub.SendMessage(reservaView);

            ViewUsuarioDto usuario = await usuarioApplication.GetByIdAsync(pedido.UsuarioId);
            if (user != null)
                await CriarEmail("Olá {{UserName}}, troca de reservas do pedido {{OrderNumber}} foi realizado (Objetivo Eventos).", "OrderChanged", usuario.Nome, pedido.Numero.ToString(), novaReserva.Evento.Nome, new List<string> { usuario.Email });

            return pedidoRetorno;
        }

        public async Task<ViewPedidoDto> PutSituacaoPedidoAsync(PutSituacaoPedidoDto putSituacaoPedidoDto)
        {
            ViewPedidoDto pedido = mapper.Map<ViewPedidoDto>(await pedidoService.PutSituacaoPedidoAsync(putSituacaoPedidoDto.Id, putSituacaoPedidoDto.SituacaoPedido));

            if (pedido is null)
                return null;

            switch (pedido.SituacaoPedido)
            {
                case SituacaoPedido.PagamentoRealizado:
                    await PagamentoRealizado(pedido);
                    break;

                case SituacaoPedido.PagamentoAprovado:
                    await PagamentoAprovado(pedido);
                    break;

                case SituacaoPedido.Estorno:
                    await Estorno(pedido);
                    break;

                case SituacaoPedido.Cancelado:
                    await Cancelado(pedido);
                    break;
            }

            return pedido;
        }

        public async Task CriarEmail(string subject, string template, string userName, string numeroPedido, string evento, List<string> usersEmail)
        {
            UserEmailOptions options = new()
            {
                ToEmails = usersEmail,
                PlaceHolders = new()
                    {
                        new KeyValuePair<string, string>("{{UserName}}",  userName),
                        new KeyValuePair<string, string>("{{OrderNumber}}",  numeroPedido),
                        new KeyValuePair<string, string>("{{OrderEvent}}",  evento),
                        new KeyValuePair<string, string>("{{Year}}",  DateTime.Now.Year.ToString())
                    },
                Subject = subject
            };

            await emailApplication.SendEmail(options, template);
        }

        public async Task CriarEmail(string subject, string template, string userName, string numeroPedido, List<string> usersEmail)
        {
            UserEmailOptions options = new()
            {
                ToEmails = usersEmail,
                PlaceHolders = new()
                    {
                        new KeyValuePair<string, string>("{{UserName}}",  userName),
                        new KeyValuePair<string, string>("{{OrderNumber}}",  numeroPedido),
                        new KeyValuePair<string, string>("{{Year}}",  DateTime.Now.Year.ToString())
                    },
                Subject = subject
            };

            await emailApplication.SendEmail(options, template);
        }

        private async Task PagamentoRealizado(ViewPedidoDto pedido)
        {
            ViewUsuarioDto usuario = await usuarioApplication.GetByIdAsync(pedido.UsuarioId);
            if (user != null)
                await CriarEmail("Olá {{UserName}}, o pagamento do pedido {{OrderNumber}} foi realizado (Objetivo Eventos).", "PaymentDone", usuario.Nome, pedido.Numero.ToString(), new List<string> { usuario.Email });
        }

        private async Task PagamentoAprovado(ViewPedidoDto pedido)
        {
            List<Guid> Ids = new();
            foreach (ReferenciaReservaDto reservaDto in pedido.Reservas)
            {
                Ids.Add(reservaDto.Id);
            }

            await reservaApplication.PutSituacaoReservaAsync(new PutSituacaoReservaDto(Ids, SituacaoReserva.CompraFinalizada));

            ViewUsuarioDto usuario = await usuarioApplication.GetByIdAsync(pedido.UsuarioId);
            if (user != null)
                await CriarEmail("Olá {{UserName}}, o pagamento do pedido {{OrderNumber}} foi aprovado (Objetivo Eventos).", "OrderCompleted", usuario.Nome, pedido.Numero.ToString(), new List<string> { usuario.Email });
        }

        private async Task Estorno(ViewPedidoDto pedido)
        {
            await Task.CompletedTask;
        }

        private async Task Cancelado(ViewPedidoDto pedido)
        {
            List<Guid> Ids = new();
            foreach (ReferenciaReservaDto reservaDto in pedido.Reservas)
            {
                Ids.Add(reservaDto.Id);
            }

            ViewUsuarioDto usuario = await usuarioApplication.GetByIdAsync(pedido.UsuarioId);
            if (user != null)
                await CriarEmail("Olá {{UserName}}, o seu pedido {{OrderNumber}} foi cancelado (Objetivo Eventos).", "OrderCanceled", usuario.Nome, pedido.Numero.ToString(), new List<string> { usuario.Email });

            await reservaApplication.PutSituacaoReservaAsync(new PutSituacaoReservaDto(Ids, SituacaoReserva.Cancelada));
            await reservaApplication.PutStatusRangeAsync(Ids, Status.Inativo);
        }

        public bool ValidarId(Guid id)
        {
            return pedidoService.ValidarId(id);
        }

        public bool ValidarInputReservaEventoPedidos(PostPedidoDto pedido)
        {
            return pedidoService.ValidarInputReservaEventoPedidos(mapper.Map<Pedido>(pedido));
        }

        public bool ValidarReservaPedidos(PostPedidoDto pedido)
        {
            return pedidoService.ValidarReservaPedidos(mapper.Map<Pedido>(pedido));
        }
    }
}