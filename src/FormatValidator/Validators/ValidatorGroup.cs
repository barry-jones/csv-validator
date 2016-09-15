using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class ValidatorGroup : ValidationEntry
    {
        private List<IValidator> _validators;

        public ValidatorGroup()
        {
            _validators = new List<IValidator>();
        }

        public ValidatorGroup(IList<IValidator> group)
        {
            _validators = new List<IValidator>();
            _validators.AddRange(group);
        }

        public override bool IsValid(string toCheck)
        {
            bool isValid = true;

            foreach(IValidator current in _validators)
            {
                bool currentValid = current.IsValid(toCheck);
                Errors.AddRange(current.GetErrors());
                isValid = isValid & currentValid;
            }

            return isValid;
        }

        public override void ClearErrors()
        {
            base.ClearErrors();
            foreach(IValidator current in _validators)
            {
                current.ClearErrors();
            }
        }

        public void Add(IValidator validator)
        {
            _validators.Add(validator);
        }

        public object Count()
        {
            return _validators.Count();
        }
    }
}
