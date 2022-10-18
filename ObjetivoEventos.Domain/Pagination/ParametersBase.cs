using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Pagination
{
    public class ParametersBase
    {
        public List<Guid> Id { get; set; }

        public Status Status { get; set; }

        private const int tamanhoMaximoResultados = 150;

        private int resultadosExibidos = 15;

        private int numeroPagina = 1;

        public int NumeroPagina
        {
            get
            {
                return numeroPagina;
            }
            set
            {
                numeroPagina = (value <= 0) ? numeroPagina : value;
            }
        }

        public int ResultadosExibidos
        {
            get
            {
                return resultadosExibidos;
            }
            set
            {
                resultadosExibidos = value == 0 ? resultadosExibidos : value <= tamanhoMaximoResultados ? value : tamanhoMaximoResultados;
            }
        }
    }
}