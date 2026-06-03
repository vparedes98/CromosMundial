using System.ComponentModel.DataAnnotations;

namespace CromosMundial.Models.Validations
{
    public class NoFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int anio)
            {
                if (anio > DateTime.Now.Year)
                {
                    return new ValidationResult("El año no puede ser mayor al año actual.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
