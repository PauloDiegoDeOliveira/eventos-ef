using ObjetivoEventos.Domain.Entitys.Base;
using System;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Endereco : EntityBase
    {
        public Guid UsuarioId { get; private set; }

        public string CEP { get; private set; }

        public string Estado { get; private set; }

        public string Cidade { get; private set; }

        public string Logradouro { get; private set; }

        public string Numero { get; private set; }

        public string Complemento { get; private set; }
    }
}