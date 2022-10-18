using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjetivoEventos.Domain.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int PaginaAtual { get; set; }

        public int TotalPaginas { get; set; }

        public int ResultadosExibidosPagina { get; set; }

        public int ContagemTotalResultados { get; set; }

        public bool ExistePaginaAnterior => PaginaAtual > 1 && TotalPaginas > 1;

        public bool ExistePaginaPosterior => PaginaAtual < TotalPaginas;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            ContagemTotalResultados = count;
            ResultadosExibidosPagina = pageSize;
            PaginaAtual = pageNumber;
            TotalPaginas = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = query.Count();

            //if (pageSize == 0)
            //    pageSize = count;

            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}