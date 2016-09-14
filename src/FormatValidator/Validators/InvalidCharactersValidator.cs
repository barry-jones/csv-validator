using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Validators
{
    public class InvalidCharactersValidator : IValidator
    {
        private List<Char> _characters;
        private List<ValidationError> _errors;

        public InvalidCharactersValidator()
        {
            _characters = new List<char>();
            _errors = new List<ValidationError>();
        }

        public bool IsValid(string toCheck)
        {
            bool isValid = true;

            foreach(char current in _characters)
            {
                if (toCheck.Contains(current.ToString()))
                {
                    _errors.Add(new ValidationError(toCheck.IndexOf(current) + 1, string.Format("'{0}' invalid character found.", current)));
                    isValid = false;
                }
            }

            return isValid;
        }

        public IList<ValidationError> GetErrors()
        {
            return _errors;
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
    }
}
