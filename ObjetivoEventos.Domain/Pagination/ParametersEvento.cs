using ObjetivoEventos.Domain.Enums;

namespace ObjetivoEventos.Domain.Pagination
{
    public class ParametersEvento : ParametersBase
    {
        public string PalavraChave { get; set; }

        public Ordenar Ordenar { get; set; }
    }
}