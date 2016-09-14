using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class StringLengthValidator : IValidator
    {
        private int _maxLength;
        private List<ValidationError> _errors;

        public StringLengthValidator(int maxLength)
        {
            _errors = new List<FormatValidator.ValidationError>();
            _maxLength = maxLength;
        }

        public bool IsValid(string toCheck)
        {
            bool isValid = toCheck.Length <= _maxLength;
            if(!isValid)
            {
                _errors.Add(new ValidationError(0, string.Format("Max length is {0}, found length was {1}.", _maxLength, toCheck.Length)));
            }

            return isValid;
        }

        public IList<ValidationError> GetErrors()
        {
            return _errors;
        }
    }
}
