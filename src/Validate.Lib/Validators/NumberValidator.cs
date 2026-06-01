
using System.Globalization;

namespace FormatValidator.Validators
{
    internal class NumberValidator : ValidationEntry
    {
        public override bool IsValid(string toCheck)
        {
            double parsed = 0;
            bool isValid = string.IsNullOrWhiteSpace(toCheck) || double.TryParse(toCheck, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed);

            if(!isValid)
            {
                base.Errors.Add(new ValidationError(0, string.Format("Could not convert '{0}' to a number.", toCheck)));
            }

            return isValid;
        }
    }
}
