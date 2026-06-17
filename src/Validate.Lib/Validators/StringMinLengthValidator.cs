
namespace FormatValidator.Validators
{
    internal class StringMinLengthValidator : ValidationEntry
    {
        private int _minLength;

        public StringMinLengthValidator(int minLength)
        {
            _minLength = minLength;
        }

        public override bool IsValid(string toCheck)
        {
            int length = toCheck == null ? 0 : toCheck.Length;
            bool isValid = length >= _minLength;

            if (!isValid)
            {
                string value = toCheck ?? string.Empty;
                base.Errors.Add(new ValidationError(0, string.Format("The content '{0}' is less than {1} characters", value, _minLength)));
            }

            return isValid;
        }
    }
}
