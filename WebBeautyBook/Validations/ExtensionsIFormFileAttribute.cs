using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Validations
{
    /// <summary>
    /// Custom validation attribute to check if the uploaded file has an allowed file extension.
    /// </summary>
    public class ExtensionsIFormFileAttribute : ValidationAttribute
    {
        private readonly List<string> _extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionsIFormFileAttribute"/> class.
        /// </summary>
        /// <param name="fileExtensions">A comma-separated list of allowed file extensions.</param>
        public ExtensionsIFormFileAttribute(string fileExtensions)
        {
            _extensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Validates that the uploaded file has an allowed file extension.
        /// </summary>
        /// <param name="value">The value being validated, which should be an instance of <see cref="IFormFile"/>.</param>
        /// <returns>True if the file has an allowed extension; otherwise, false.</returns>
        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;
            if(file == null) return true;

            return _extensions.Any(y => file.FileName.EndsWith(y));
        }
    }
}
