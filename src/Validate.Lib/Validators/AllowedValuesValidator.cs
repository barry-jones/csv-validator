
namespace FormatValidator.Validators
{
    using System.Collections.Generic;

    internal class AllowedValuesValidator : ValidationEntry
    {
        private List<string> _allowedValues;

        public AllowedValuesValidator(IReadOnlyList<string> allowedValues)
        {
            _allowedValues = new List<string>(allowedValues);
        }

        public override bool IsValid(string toCheck)
        {
            if (string.IsNullOrEmpty(toCheck))
                return true;

            bool isValid = _allowedValues.Contains(toCheck);
            if (!isValid)
            {
                base.Errors.Add(new ValidationError(0, string.Format(
                    "Value '{0}' is not in the list of allowed values [{1}].",
                    toCheck,
                    string.Join(", ", _allowedValues))));
            }

            return isValid;
        }
    }
}
