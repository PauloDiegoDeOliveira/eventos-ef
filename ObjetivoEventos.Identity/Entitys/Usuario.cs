using Microsoft.AspNetCore.Identity;
using System;

namespace ObjetivoEventos.Identity.Entitys
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string RA { get; set; }

        public string Apelido { get; set; }

        public string CPF { get; set; }

        public DateTime Nascimento { get; set; }

        public string Genero { get; set; }

        public string Status { get; set; }

        public DateTime CriadoEm { get; set; }

        public DateTime? AlteradoEm { get; set; }
    }
}