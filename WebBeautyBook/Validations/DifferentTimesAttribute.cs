using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Validations
{
    public class DifferentTimesAttribute : ValidationAttribute
    {

        private readonly string _otherTime;

        public DifferentTimesAttribute(string otherTime)
        {
            _otherTime = otherTime;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endTime = (TimeOnly)value;

            var startTimeProperty = validationContext.ObjectType.GetProperty(_otherTime);

            if (startTimeProperty == null)
                return new ValidationResult("Second time cannot be null.");

            var startTimeValue = (TimeOnly)startTimeProperty.GetValue(validationContext.ObjectInstance);

            if (endTime == startTimeValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

    }
}
