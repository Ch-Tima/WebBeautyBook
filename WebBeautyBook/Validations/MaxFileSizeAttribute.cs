using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Validations
{
    /// <summary>
    /// Custom validation attribute to check if the size of an uploaded file does not exceed a specified maximum file size.
    /// </summary>
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxFileSizeAttribute"/> class.
        /// </summary>
        /// <param name="maxFileSize">The maximum allowed file size in bytes.</param>
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        /// <summary>
        /// Validates that the size of the uploaded file does not exceed the specified maximum file size.
        /// </summary>
        /// <param name="value">The value being validated, which should be an instance of <see cref="IFormFile"/>.</param>
        /// <returns>True if the file size is within the allowed limit; otherwise, false.</returns>
        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;
            if (file == null) return true;
            if (file.Length > _maxFileSize) return false;
            return true;
        }
    }
}
