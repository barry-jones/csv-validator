using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class StringLengthValidator
    {
        private int _maxLength;

        public StringLengthValidator()
        {
            _maxLength = 0;
        }

        public bool IsValid(string toCheck)
        {
            return toCheck.Length <= MaxLength;
        }

        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }
    }
}
