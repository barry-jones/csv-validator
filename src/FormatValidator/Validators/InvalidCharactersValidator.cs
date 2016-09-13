using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class InvalidCharactersValidator : IValidator
    {
        private List<Char> _characters;
        private List<string> _errors;

        public InvalidCharactersValidator()
        {
            _characters = new List<char>();
            _errors = new List<string>();
        }

        public bool IsValid(string toCheck)
        {
            bool isValid = true;

            foreach(char current in _characters)
            {
                if (toCheck.Contains(current.ToString()))
                {
                    _errors.Add(string.Format("'{0}' at {1}", current, toCheck.IndexOf(current) + 1));
                    isValid = false;
                }
            }

            return isValid;
        }

        public List<char> Characters
        {
            get
            {
                return _characters;
            }
            set
            {
                _characters = value;
            }
        }

        public string GetErrors()
        {
            return string.Join(", ", _errors);
        }
    }
}
