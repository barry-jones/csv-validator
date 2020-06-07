
namespace FormatValidator.Validators
{
    using System.Text.RegularExpressions;

    internal class TextFormatValidator : ValidationEntry
    {
        private Regex _format;

        public TextFormatValidator(string format)
        {
            _format = new Regex(format);
        }

        public override bool IsValid(string toCheck)
        {
            bool isValid = string.IsNullOrWhiteSpace(toCheck) || _format.IsMatch(toCheck);
            if(!isValid)
            {
                base.Errors.Add(new ValidationError(0, string.Format("String '{0}' was not in correct format.", toCheck)));
            }
            return isValid;
        }
    }
}
