using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Library.Models.ValidationAttributes
{
    public class IdIntegerAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            var defaultErrorMessage = "You can use only numbers";

            return string.IsNullOrEmpty(ErrorMessage)
                ? defaultErrorMessage
                : ErrorMessage;
        }

        public override bool IsValid(object? value)
        {
            if (value is not string)
            {
                throw new ArgumentException($"Wrong type for validation. {nameof(IsbnFormatAttribute)} can work only with string");
            }

            
            return value is int;
        }
    }
}
