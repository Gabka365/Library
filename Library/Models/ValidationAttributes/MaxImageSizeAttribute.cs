using System.ComponentModel.DataAnnotations;
using System.Drawing;


namespace Library.Models.ValidationAttributes
{
    public class MaxImageSizeAttribute : ValidationAttribute
    {
        private int _maxImageWidth;
        private int _maxImageHeight;

        public MaxImageSizeAttribute(int maxImageWidth, int maxImageHeight)
        {
            _maxImageWidth = maxImageWidth;
            _maxImageHeight = maxImageHeight;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is not IFormFile)
                {
                    throw new ArgumentException($"You can use {nameof(MaxImageSizeAttribute)} only with IFormFile ");
                }

                var formFile = (IFormFile)value;

                using var stream = formFile.OpenReadStream();
                var image = Image.FromStream(stream);

                if (image.Width > _maxImageWidth || image.Height > _maxImageHeight)
                {
                    return new ValidationResult(
                        $"You can use only images with {_maxImageWidth}px width and {_maxImageHeight}px height.");
                }

                return ValidationResult.Success;
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
