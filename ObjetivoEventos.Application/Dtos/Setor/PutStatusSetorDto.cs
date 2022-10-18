using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusSetorDto : PutStatusDto
    {
        public PutStatusSetorDto()
        { }

        public PutStatusSetorDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}