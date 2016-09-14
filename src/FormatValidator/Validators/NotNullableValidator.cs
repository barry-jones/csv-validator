using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class NotNullableValidator : IValidator
    {
        private List<ValidationError> _errors;

        public NotNullableValidator()
        {
            _errors = new List<ValidationError>();
        }

        public bool IsValid(string toCheck)
        {
            bool isValid = !string.IsNullOrEmpty(toCheck);
            if(!isValid)
            {
                _errors.Add(new ValidationError(0, string.Format("Expected null, actually saw '{0}'.", toCheck)));
            }

            return isValid;
        }

        public IList<ValidationError> GetErrors()
        {
            throw new NotImplementedException();
        }
    }
}
