using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Application.Utilities.Caminhos;
using ObjetivoEventos.Application.Utilities.Imagem;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications
{
    public class EventoApplication : ApplicationBase<Evento, ViewEventoDto, PostEventoDto, PutEventoDto, PutStatusDto>, IEventoApplication
    {
        private readonly IEventoService eventoService;

        public EventoApplication(IEventoService eventoService,
                                 INotificador notificador,
                                 IMapper mapper) : base(eventoService, notificador, mapper)
        {
            this.eventoService = eventoService;
        }

        public async Task<ViewEventoDisponibilidadeDto> GetDisponibilidadeAsync(Guid eventoId, Guid setorId)
        {
            Evento evento = await eventoService.GetDisponibilidadeAsync(eventoId, setorId);

            if (evento is null)
                return null;

            ViewSetorDisponibilidadeDto viewSetorDisponibilidadeDto = new(evento.Local.Setores[0]);

            foreach (Reserva reserva in evento.Reservas)
            {
                ViewCadeiraDisponibilidadeDto viewCadeiraDisponibilidade;

                if (reserva.MesaId == Guid.Empty || reserva.MesaId == null)
                {
                    viewCadeiraDisponibilidade = viewSetorDisponibilidadeDto.Cadeiras.Find(x => x.Id == reserva.CadeiraId);

                    if (viewCadeiraDisponibilidade is not null)
                    {
                        viewSetorDisponibilidadeDto.Cadeiras.Find(x => x.Id == reserva.CadeiraId).AlterarDisponibilidade(StatusDisponibilidade.Indisponivel);
                    }
                }
                else
                {
                    ViewMesaDisponibilidadeDto viewMesaDisponibilidade = viewSetorDisponibilidadeDto.Mesas.Find(x => x.Id == reserva.MesaId);
                    if (viewMesaDisponibilidade is not null)
                    {
                        viewCadeiraDisponibilidade = viewMesaDisponibilidade.Cadeiras.Find(x => x.Id == reserva.CadeiraId);

                        if (viewCadeiraDisponibilidade is not null)
                        {
                            viewSetorDisponibilidadeDto.Mesas.Find(x => x.Id == reserva.MesaId).Cadeiras.Find(y => y.Id == reserva.CadeiraId).AlterarDisponibilidade(StatusDisponibilidade.Indisponivel);
                        }
                    }
                }

                viewSetorDisponibilidadeDto.EstaDisponivel();
            }

            return new ViewEventoDisponibilidadeDto(evento, new ViewLocalDisponibilidadeDto(evento.Local, viewSetorDisponibilidadeDto), mapper.Map<List<ViewReservaDto>>(evento.Reservas));
        }

        public async Task<ViewEventoDetalhesDto> GetDetalhesAsync(Guid eventoId)
        {
            return mapper.Map<ViewEventoDetalhesDto>(await eventoService.GetDetalhesAsync(eventoId));
        }

        public async Task<ViewPagedListDto<Evento, ViewEventoDto>> GetPaginationAsync(ParametersEvento parametersEvento)
        {
            PagedList<Evento> pagedList = await eventoService.GetPaginationAsync(parametersEvento);
            return new ViewPagedListDto<Evento, ViewEventoDto>(pagedList, mapper.Map<List<ViewEventoDto>>(await eventoService.GetPaginationAsync(parametersEvento)));
        }

        public async Task<List<ViewEventoDto>> GetEventosExpiradosAsync()
        {
            return mapper.Map<List<ViewEventoDto>>(await eventoService.GetEventosExpiradosAsync());
        }

        public async Task<ViewEventoDto> PostAsync(PostEventoDto postEventoDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo)
        {
            Evento objeto = mapper.Map<Evento>(postEventoDto);

            objeto.PolulateInformations(objeto, await PathCreator.CreateIpPath(caminhoFisico),
                                                await PathCreator.CreateAbsolutePath(caminhoAbsoluto),
                                                await PathCreator.CreateRelativePath(await PathCreator.CreateAbsolutePath(caminhoAbsoluto), splitRelativo));

            UploadIFormFileMethods<Evento> uploadClass = new();
            await uploadClass.UploadImage(objeto);

            return mapper.Map<ViewEventoDto>(await eventoService.PostAsync(objeto));
        }

        public async Task<ViewEventoDto> PutAsync(PutEventoDto putEventoDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo)
        {
            Evento objeto = mapper.Map<Evento>(putEventoDto);
            Evento consulta = await eventoService.GetByIdAsync(putEventoDto.Id);

            if (consulta is null)
                return null;

            if (putEventoDto.ImagemUpload is not null)
            {
                UploadIFormFileMethods<Evento> uploadClass = new();
                await uploadClass.DeleteImage(consulta);

                objeto.PolulateInformations(objeto, await PathCreator.CreateIpPath(caminhoFisico),
                                                    await PathCreator.CreateAbsolutePath(caminhoAbsoluto),
                                                    await PathCreator.CreateRelativePath(await PathCreator.CreateAbsolutePath(caminhoAbsoluto), splitRelativo));

                await uploadClass.UploadImage(objeto);
            }
            else
            {
                objeto.PutInformations(consulta);
            }

            return mapper.Map<ViewEventoDto>(await eventoService.PutAsync(objeto));
        }

        public async Task<List<ViewEventoDto>> PutStatusRangeAsync(List<Guid> ids, Status status)
        {
            return mapper.Map<List<ViewEventoDto>>(await eventoService.PutStatusRangeAsync(ids, status));
        }

        public bool ValidarEventoExpirado(Guid id)
        {
            return eventoService.ValidarEventoExpirado(id);
        }

        public bool ValidarId(Guid id)
        {
            return eventoService.ValidarId(id);
        }

        public bool ValidarDataHoraPost(PostEventoDto postEventoDto)
        {
            return mapper.Map<bool>(eventoService.ValidarDataHora(mapper.Map<Evento>(postEventoDto)));
        }

        public bool ValidarDataHoraPut(PutEventoDto putEventoDto)
        {
            return mapper.Map<bool>(eventoService.ValidarDataHora(mapper.Map<Evento>(putEventoDto)));
        }
    }
}