using ObjetivoEventos.Domain.Pagination;

namespace ObjetivoEventos.Application.Dtos.Pagination
{
    /// <summary>
    /// Objeto utilizado para visualização dos dados da paginação.
    /// </summary>
    public class ViewPaginationDataDto<T>
    {
        public int PaginaAtual { get; private set; }

        public int TotalPaginas { get; private set; }

        public int ResultadosExibidosPagina { get; private set; }

        public int ContagemTotalResultados { get; private set; }

        public bool ExistePaginaAnterior { get; private set; }

        public bool ExistePaginaPosterior { get; private set; }

        public ViewPaginationDataDto()
        {
        }

        public ViewPaginationDataDto(PagedList<T> pagedList)
        {
            ContagemTotalResultados = pagedList.ContagemTotalResultados;

            ResultadosExibidosPagina = pagedList.ResultadosExibidosPagina;

            PaginaAtual = pagedList.PaginaAtual;

            TotalPaginas = pagedList.TotalPaginas;

            ExistePaginaPosterior = pagedList.ExistePaginaPosterior;

            ExistePaginaAnterior = pagedList.ExistePaginaAnterior;
        }
    }
}