using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class TextFormatValidator : IValidator
    {
        private Regex _format;

        public TextFormatValidator(string format)
        {
            _format = new Regex(format);
        }

        public bool IsValid(string toCheck)
        {
            return _format.IsMatch(toCheck);
        }
    }
}
