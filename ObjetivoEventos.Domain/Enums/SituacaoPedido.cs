namespace ObjetivoEventos.Domain.Enums
{
    public enum SituacaoPedido
    {
        AguardandoPagamento = 1,
        PagamentoRealizado,
        PagamentoAprovado,
        Estorno,
        Cancelado
    }
}