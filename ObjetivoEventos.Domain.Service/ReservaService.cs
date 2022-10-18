﻿using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class ReservaService : ServiceBase<Reserva>, IReservaService
    {
        private readonly IReservaRepository reservaRepository;

        public ReservaService(IReservaRepository reservaRepository) : base(reservaRepository)
        {
            this.reservaRepository = reservaRepository;
        }

        public async Task<PagedList<Reserva>> GetPaginationAsync(ParametersBase parametersBase)
        {
            return await reservaRepository.GetPaginationAsync(parametersBase);
        }

        public async Task<Reserva> GetReservasDetalhesById(Guid id)
        {
            return await reservaRepository.GetReservasDetalhesById(id);
        }

        public async Task<List<Reserva>> GetReservaByTempoSituacaoAsync(int minutos, SituacaoReserva situacaoReserva)
        {
            return await reservaRepository.GetReservasByTempoSituacaoAsync(minutos, situacaoReserva);
        }

        public async Task<List<Reserva>> PutSituacaoReservaAsync(List<Guid> ids, SituacaoReserva situacaoReserva)
        {
            List<Reserva> reservasConsultadas = await reservaRepository.GetReservasByListId(ids);

            if (reservasConsultadas == null)
                return null;

            foreach (Reserva reserva in reservasConsultadas)
            {
                if (reserva.VerificaRegraSituacaoReserva(situacaoReserva.ToString()))
                {
                    reserva.AlterarSituacaoReserva(situacaoReserva.ToString());
                }
            }

            return await reservaRepository.PutRangeAsync(reservasConsultadas);
        }

        public async Task<List<Reserva>> PutStatusRangeAsync(List<Guid> ids, Status status)
        {
            return await reservaRepository.PutStatusRangeAsync(ids, status);
        }

        public async Task<List<Reserva>> DeleteRangeAsync(List<Reserva> reservas)
        {
            List<Reserva> retorno = await reservaRepository.DeleteRangeAsync(reservas);

            foreach (Reserva reserva in retorno)
            {
                reserva.AlterarSituacaoReserva(SituacaoReserva.Cancelada.ToString());
            }

            return retorno;
        }

        public async Task<List<Reserva>> DeleteByConnectionId(string connectionId)
        {
            List<Reserva> reservas = await reservaRepository.DeleteByConnectionId(connectionId);

            foreach (Reserva reserva in reservas)
            {
                reserva.AlterarSituacaoReserva(SituacaoReserva.Cancelada.ToString());
            }

            return reservas;
        }

        public override async Task<Reserva> DeleteAsync(Guid id)
        {
            Reserva reserva = await base.DeleteAsync(id);
            reserva.AlterarSituacaoReserva(SituacaoReserva.Cancelada.ToString());
            return reserva;
        }

        public bool VerificaDisponibilidadeReserva(Reserva reserva)
        {
            return reservaRepository.VerificaDisponibilidadeReserva(reserva);
        }

        public bool ValidarSetorCadeiraMesa(Reserva reserva)
        {
            return reservaRepository.ValidarSetorCadeiraMesa(reserva);
        }

        public bool ValidaListId(List<Guid> ids)
        {
            return reservaRepository.ValidaListId(ids);
        }

        public bool ValidaQuantidadeCadeiras(Reserva reserva)
        {
            return reservaRepository.ValidaQuantidadeCadeiras(reserva);
        }

        public bool ValidarId(Guid id)
        {
            return reservaRepository.ValidarId(id);
        }
    }
}