using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Validations
{
    public class ExtensionsIFormFileAttribute : ValidationAttribute
    {
        private readonly List<string> _extensions;
        public ExtensionsIFormFileAttribute(string fileExtensions)
        {
            _extensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;

            if(file == null) return true;

            return _extensions.Any(y => file.FileName.EndsWith(y));
        }

    }
}
