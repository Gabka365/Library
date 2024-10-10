using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Library.Models.ValidationAttributes
{
    public class BooksCountAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            var defaultErrorMessage = "Count of books need to be more than 0.";

            return string.IsNullOrEmpty(ErrorMessage)
                ? defaultErrorMessage
                : ErrorMessage;
        }

        public override bool IsValid(object? value)
        {
            if (value is not int)
            {
                throw new ArgumentException($"Wrong type for validation. {nameof(BooksCountAttribute)} can work only with string");
            }

            var count = (int)value;
            
            return count >= 0;
        }
    }
}
