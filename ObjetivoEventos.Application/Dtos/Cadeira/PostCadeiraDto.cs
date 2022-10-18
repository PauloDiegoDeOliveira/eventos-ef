using ObjetivoEventos.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Cadeira
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostCadeiraDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Cadeira</example>
        [Display(Name = "Nome da cadeira.")]
        public string Nome { get; set; }

        /// <summary>
        /// Fileira
        /// </summary>
        /// <example>A</example>
        [Display(Name = "Fileira da cadeira.")]
        [Required(ErrorMessage = "O campo fileira da cadeira é obrigatório.")]
        [EnumDataType(typeof(Fileira), ErrorMessage = "A fileira é inválida.")]
        public Fileira Fileira { get; set; }

        /// <summary>
        /// Coluna
        /// </summary>
        /// <example>1</example>
        [Display(Name = "Coluna da cadeira.")]
        [Required(ErrorMessage = "O campo coluna da cadeira é obrigatório.")]
        public int Coluna { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status da cadeira.")]
        [Required(ErrorMessage = "O campo status da cadeira é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }

        public PostCadeiraDto()
        {
        }

        public PostCadeiraDto(string nome, Fileira fileira, int coluna, Status status)
        {
            Nome = nome;
            Fileira = fileira;
            Coluna = coluna;
            Status = status;
        }
    }
}