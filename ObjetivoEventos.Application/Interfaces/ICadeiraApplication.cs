using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface ICadeiraApplication : IApplicationBase<Cadeira, ViewCadeiraDto, PostCadeiraDto, PutCadeiraDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Cadeira, ViewCadeiraDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<ViewCadeiraDto>> PostAutomaticoAsync(List<PostCadeiraDto> postCadeiraDtos);

        bool ValidarId(Guid id);

        bool ValidarFileiraEColunaPost(PostCadeiraDto postCadeiraDto);

        bool ValidarFileiraEColunaPut(PutCadeiraDto putCadeiraDto);
    }
}