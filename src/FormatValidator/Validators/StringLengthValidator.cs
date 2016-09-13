using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class StringLengthValidator : IValidator
    {
        private int _maxLength;

        public StringLengthValidator(int maxLength)
        {
            _maxLength = maxLength;
        }

        public bool IsValid(string toCheck)
        {
            return toCheck.Length <= _maxLength;
        }
    }
}
