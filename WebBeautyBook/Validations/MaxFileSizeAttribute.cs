using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;
            if (file == null) return true;

            if (file.Length > _maxFileSize) return false;

            return true;
        }
    }
}
