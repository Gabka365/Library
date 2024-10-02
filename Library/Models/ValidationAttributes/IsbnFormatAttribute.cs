using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Library.Models.ValidationAttributes
{
    public class IsbnFormatAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            var defaultErrorMessage = "Correct format is 000-0-000-00000-0";

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

            var isbn = (string) value;

            var isbnRegex = new Regex(@"^\d{3}-\d-\d{3}-\d{5}-\d$");

            return isbnRegex.IsMatch(isbn);
        }
    }
}
