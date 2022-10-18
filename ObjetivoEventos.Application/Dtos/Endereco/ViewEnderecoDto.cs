using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Endereco
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewEnderecoDto
    {
        public Guid Id { get; set; }

        public string CEP { get; set; }

        public Estado Estado { get; set; }

        public string Cidade { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public Status Status { get; set; }
    }
}