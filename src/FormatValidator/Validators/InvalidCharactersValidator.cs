using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class InvalidCharactersValidator : ValidationEntry
    {
        private List<Char> _characters;

        public InvalidCharactersValidator()
        {
            _characters = new List<char>();
        }

        public override bool IsValid(string toCheck)
        {
            bool isValid = true;

            foreach(char current in _characters)
            {
                if (toCheck.Contains(current.ToString()))
                {
                    base.Errors.Add(new ValidationError(toCheck.IndexOf(current) + 1, string.Format("'{0}' invalid character found.", current)));
                    isValid = false;
                }
            }

            return isValid;
        }

        public void Add(char[] invalidCharacters)
        {
            _characters.AddRange(invalidCharacters);
        }
    }
}
