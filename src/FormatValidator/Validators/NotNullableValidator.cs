using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class NotNullableValidator : ValidationEntry
    {
        public override bool IsValid(string toCheck)
        {
            bool isValid = !string.IsNullOrEmpty(toCheck);
            if(!isValid)
            {
                Errors.Add(new ValidationError(0, string.Format("Expected a value, actually saw '{0}'.", toCheck)));
            }

            return isValid;
        }
    }
}
