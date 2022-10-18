namespace ObjetivoEventos.Domain.Enums
{
    public enum SituacaoReserva
    {
        Reservado = 1,
        AguardandoPagamento,
        CompraFinalizada,
        PagamentoNaoFinalizado,
        Utilizada,
        Cancelada
    }
}