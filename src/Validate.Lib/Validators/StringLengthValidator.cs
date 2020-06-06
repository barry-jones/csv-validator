
namespace FormatValidator.Validators
{
    public class StringLengthValidator : ValidationEntry
    {
        private int _maxLength;

        public StringLengthValidator(int maxLength)
        {
            _maxLength = maxLength;
        }

        public override bool IsValid(string toCheck)
        {
            bool isValid = toCheck.Length <= _maxLength;
            if(!isValid)
            {
                base.Errors.Add(new ValidationError(0, string.Format("Max length is {0}, found length was {1}.", _maxLength, toCheck.Length)));
            }

            return isValid;
        }
    }
}
