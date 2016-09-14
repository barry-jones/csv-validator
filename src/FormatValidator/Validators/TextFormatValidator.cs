using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class TextFormatValidator : Validator
    {
        private Regex _format;

        public TextFormatValidator(string format)
        {
            _format = new Regex(format);
        }

        public override bool IsValid(string toCheck)
        {
            bool isValid = _format.IsMatch(toCheck);
            if(!isValid)
            {
                Errors.Add(new ValidationError(0, string.Format("String {0} was not in correct format.", toCheck)));
            }
            return _format.IsMatch(toCheck);
        }
    }
}
