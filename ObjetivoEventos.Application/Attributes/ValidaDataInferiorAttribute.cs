using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application
{
    public class ValidaDataInferiorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;

            if (dt <= DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}