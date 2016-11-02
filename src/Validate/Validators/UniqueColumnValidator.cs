
namespace FormatValidator.Validators
{
    using System.Collections.Generic;

    // this class needs a persitant error management, so that it can last through multiple
    // row validations.

    public class UniqueColumnValidator : ValidationEntry
    {
        private List<string> _entries;

        public UniqueColumnValidator()
        {
            _entries = new List<string>();
        }

        public override bool IsValid(string toCheck)
        {
            bool isValid = !_entries.Contains(toCheck);

            if(isValid)
            {
                _entries.Add(toCheck);
            }
            else
            {
                base.Errors.Add(new ValidationError(0, string.Format("Duplicate ID {0} found.", toCheck)));
            }

            return isValid;
        }
    }
}
