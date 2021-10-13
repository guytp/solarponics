using System.ComponentModel.DataAnnotations;

namespace Solarponics.Models
{
    public class ValidationResponse
    {
        public ValidationResult[] ValidationFailures { get; }

        public ValidationResponse(ValidationResult[] validationFailures)
        {
            this.ValidationFailures = validationFailures;
        }
    }
}