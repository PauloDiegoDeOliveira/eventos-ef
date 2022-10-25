using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface ISetorApplication : IApplicationBase<Setor, ViewSetorDto, PostSetorDto, PutSetorDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Setor, ViewSetorDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<ViewSetorDto>> GetSetorDadosByIds(PostViewDadosSetorDto postViewDadosSetorDto);

        Task<ViewSetorDto> PostSetorByCadeirasAutomaticoAsync(PostAutomaticoSetorDto postAutomaticoSetorDto);

        Task<ViewSetorDto> PutSetorByCadeirasAutomaticoAsync(PutAutomaticoSetorDto putAutomaticoSetorDto);

        bool ValidarId(Guid id);
    }
}