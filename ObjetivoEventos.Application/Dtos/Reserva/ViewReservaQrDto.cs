using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para visualização dos dados de reserva com QRCode.
    /// </summary>
    public class ViewReservaQrDto
    {
        public Guid Id { get; set; }

        public SituacaoReserva SituacaoReserva { get; set; }

        public Status Status { get; set; }

        public ViewSetorUsuarioAutenticadoDto Setor { get; set; }

        public ViewMesaUsuarioAutenticadoDto Mesa { get; set; }

        public ViewCadeiraDto Cadeira { get; set; }

        public string QrCodeString { get; set; }

        public ViewReservaQrDto(Domain.Entitys.Reserva reserva,
            ViewSetorUsuarioAutenticadoDto Setor,
            ViewMesaUsuarioAutenticadoDto Mesa,
            ViewCadeiraDto Cadeira,
            string QrCodeString)
        {
            Id = reserva.Id;
            this.Setor = Setor;
            this.Cadeira = Cadeira;

            if (Mesa.Id != Guid.Empty)
                this.Mesa = Mesa;

            _ = Enum.TryParse(reserva.SituacaoReserva, out SituacaoReserva situacao);
            SituacaoReserva = situacao;
            _ = Enum.TryParse(reserva.Status, out Status status);
            Status = status;
            this.QrCodeString = QrCodeString;
        }
    }
}