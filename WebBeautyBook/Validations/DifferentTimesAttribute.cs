using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Validations
{
    /// <summary>
    /// Custom validation attribute to ensure that two TimeOnly properties have different values.
    /// </summary>
    public class DifferentTimesAttribute : ValidationAttribute
    {
        private readonly string _otherTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DifferentTimesAttribute"/> class.
        /// </summary>
        /// <param name="otherTime">The name of the other TimeOnly property to compare against.</param>
        public DifferentTimesAttribute(string otherTime)
        {
            _otherTime = otherTime;
        }

        /// <summary>
        /// Validates that the specified TimeOnly property has a different value than the other specified TimeOnly property.
        /// </summary>
        /// <param name="value">The value of the TimeOnly property being validated.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation succeeded or failed.</returns>
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
