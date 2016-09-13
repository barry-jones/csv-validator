using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class ValidatorGroup : IValidator
    {
        private List<IValidator> _validators;

        public ValidatorGroup()
        {
            _validators = new List<IValidator>();
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

        public void Add(IValidator validator)
        {
            _validators.Add(validator);
        }
    }
}
