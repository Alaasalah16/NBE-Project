using System.ComponentModel.DataAnnotations;

public class DateRangeAttribute : ValidationAttribute
{
   
        private readonly string _startDatePropertyName;

        public DateRangeAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = (DateTime?)value;
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);

            if (startDateProperty == null)
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");

            var startDate = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);

            // Ensure dates are not null
            if (!startDate.HasValue || !endDate.HasValue)
                return new ValidationResult("Both start and end dates are required.");

            // Validate date order
            if (startDate > endDate)
                return new ValidationResult("The start date must be earlier than the end date.");

            // Validate within a specific range (e.g., 01-Jan-2023 to 31-Dec-2050)
            var minDate = new DateTime(2023, 1, 1);
            var maxDate = new DateTime(2050, 12, 31);

            if (startDate < minDate || endDate > maxDate)
                return new ValidationResult($"The dates must be within the range {minDate:dd-MM-yyyy} to {maxDate:dd-MM-yyyy}.");

            return ValidationResult.Success;
        }
    }


