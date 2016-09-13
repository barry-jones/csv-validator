using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class NotNullableValidator : IValidator
    {
        public bool IsValid(string toCheck)
        {
            return !string.IsNullOrEmpty(toCheck);
        }
    }
}
