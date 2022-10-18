using ObjetivoEventos.Application.Dtos.Pagination;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Identity.Usuario
{
    /// <summary>
    /// Objeto utilizado para visualização páginada de usuário.
    /// </summary>
    public class ViewPaginacaoUsuarioDto
    {
        public ICollection<ViewUsuarioDto> Usuarios { get; set; }

        public ViewPaginationDataDto<ObjetivoEventos.Identity.Entitys.Usuario> DadosPaginacao { get; set; }

        public ViewPaginacaoUsuarioDto()
        {
            Usuarios = new List<ViewUsuarioDto>();
            DadosPaginacao = new ViewPaginationDataDto<ObjetivoEventos.Identity.Entitys.Usuario>();
        }
    }
}