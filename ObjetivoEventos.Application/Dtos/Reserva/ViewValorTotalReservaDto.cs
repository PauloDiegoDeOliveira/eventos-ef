
namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para visualização do valor total de reservas.
    /// </summary>
    public class ViewValorTotalReservaDto
    {
        public string ValorTotal { get; set; }

        public ViewValorTotalReservaDto() { }

        public ViewValorTotalReservaDto(string valor)
        {
            ValorTotal = valor;
        }
    }
}
