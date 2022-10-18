using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Utilities.Caminhos
{
    public static class DateInformations
    {
        public static string GetSplitData(Data date)
        {
            DateTime datevalue = DateTime.Now;

            return date switch
            {
                Data.Year => datevalue.Year.ToString(),
                Data.Month => datevalue.Month.ToString(),
                Data.Day => datevalue.Day.ToString(),
                _ => null,
            };
        }
    }
}