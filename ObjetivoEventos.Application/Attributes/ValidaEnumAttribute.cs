using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Attributes
{
    public class ValidaEnumAttribute<TEnum> : ValidationAttribute where TEnum : Enum
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.GetType() != typeof(TEnum) || !Enum.IsDefined(typeof(TEnum), value))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}