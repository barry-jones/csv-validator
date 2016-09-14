using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class NumberValidator : IValidator
    {
        private List<ValidationError> _errors;

        public NumberValidator()
        {
            _errors = new List<ValidationError>();
        }

        public bool IsValid(string toCheck)
        {
            double parsed = 0;
            bool isValid = double.TryParse(toCheck, out parsed);

            if(!isValid)
            {
                _errors.Add(new ValidationError(0, string.Format("Could not convert '{0}' to a number.", toCheck)));
            }

            return isValid;
        }

        public IList<ValidationError> GetErrors()
        {
            return _errors;
        }
    }
}
