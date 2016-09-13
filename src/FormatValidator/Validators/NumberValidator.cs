using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class NumberValidator : IValidator
    {
        public bool IsValid(string toCheck)
        {
            double parsed = 0;
            return double.TryParse(toCheck, out parsed);
        }
    }
}
