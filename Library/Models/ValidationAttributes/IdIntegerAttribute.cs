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
            
            if (value == null)
            {
                return true; 
            }

            
            if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out _))
                {
                    return true; 
                }
                return false;
            }

            return false;
        }
    }
}
