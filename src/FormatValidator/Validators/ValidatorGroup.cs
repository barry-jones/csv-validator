using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class ValidatorGroup : IValidator
    {
        private List<IValidator> _validators;
        private List<ValidationError> _errors;

        public ValidatorGroup()
        {
            _errors = new List<ValidationError>();
            _validators = new List<IValidator>();
        }

        public ValidatorGroup(IList<IValidator> group)
        {
            _validators = new List<IValidator>();
            _validators.AddRange(group);
        }

        public bool IsValid(string toCheck)
        {
            bool isValid = true;

            foreach(IValidator current in _validators)
            {
                bool currentValid = current.IsValid(toCheck);
                isValid = isValid & currentValid;
            }

            return isValid;
        }

        public IList<ValidationError> GetErrors()
        {
            return _errors;
        }

        public void Add(IValidator validator)
        {
            _validators.Add(validator);
        }
    }
}
