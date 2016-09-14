using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class NumberValidator : Validator
    {
        public override bool IsValid(string toCheck)
        {
            double parsed = 0;
            bool isValid = double.TryParse(toCheck, out parsed);

            if(!isValid)
            {
                Errors.Add(new ValidationError(0, string.Format("Could not convert '{0}' to a number.", toCheck)));
            }

            return isValid;
        }
    }
}
