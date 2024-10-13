using Library.Data.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Library.Models.ValidationAttributes
{
    public class UserCheckAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            var defaultErrorMessage = "This user already exists.";

            return string.IsNullOrEmpty(ErrorMessage)
                ? defaultErrorMessage
                : ErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string)
            {
                throw new ArgumentException($"Wrong type for validation. {nameof(UserCheckAttribute)} can work only with string.");
            }

            var userName = (string)value;

            var userRepository = (IUserRepository)validationContext.GetService(typeof(IUserRepository));

            if (userRepository == null)
            {
                throw new InvalidOperationException("UserRepository is not available.");
            }

            if (userRepository.Exist(userName))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
