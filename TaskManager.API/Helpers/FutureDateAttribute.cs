using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Helpers
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime <= DateTime.UtcNow)
                {
                    return new ValidationResult(
                        ErrorMessage ?? "Date must be in the future");
                }
            }
            return ValidationResult.Success;
        }
    }
}